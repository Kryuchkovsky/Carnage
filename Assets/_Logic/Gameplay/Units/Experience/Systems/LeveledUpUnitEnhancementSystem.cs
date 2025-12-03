using System.Collections.Generic;
using _Logic.Core;
using _Logic.Gameplay.Abilities.Components;
using _Logic.Gameplay.SelectionPanel;
using _Logic.Gameplay.SurvivalMode;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Experience.Events;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Stats.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LeveledUpUnitEnhancementSystem : AbstractUpdateSystem
    {
        private Event<LevelChangeEvent> _levelChangeEvent;
        private Request<StatModificationRequest> _statModificationRequest;
        private Filter _abilitySelectionsFilter;
        private readonly int _levelIntervalBetweenAbilitySelection = 3;

        [Inject] private Selector _selector;
        [Inject] private SurvivalModeSettings _survivalModeSettings;

        private Dictionary<Entity, LeveledUpUnitEnhancement> _enhancements = new();

        public override void OnAwake()
        {
            _levelChangeEvent = World.GetEvent<LevelChangeEvent>();
            _statModificationRequest = World.GetRequest<StatModificationRequest>();
            _abilitySelectionsFilter = World.Filter.With<AbilitySelectionsComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _levelChangeEvent.publishedChanges)
            {
                if (evt.Entity.IsNullOrDisposed() || evt.Change <= 0 || evt.NewLevel <= 1 || evt.Entity.Has<AIComponent>() || !evt.Entity.Has<StatsComponent>()) 
                    continue;
                
                ref var statsComponent = ref evt.Entity.GetComponent<StatsComponent>();
                var selections = new List<SelectionData>(_survivalModeSettings.RewardsNumberWhenLevelUp);
                var rewardsNumber = Mathf.Clamp(_survivalModeSettings.RewardsNumberWhenLevelUp, 0, _survivalModeSettings.PossibleStatModifiersWhenLevelUp.length);
                var hasSelectionOfAbilities = evt.NewLevel % _levelIntervalBetweenAbilitySelection == 0;

                if (hasSelectionOfAbilities)
                {
                    for (int i = 0; i < _survivalModeSettings.PossibleStatModifiersWhenLevelUp.length && selections.Count < rewardsNumber; i++)
                    {
                        var randomValue = Random.Range(0f, 1f);
                        var statModifier = _survivalModeSettings.PossibleStatModifiersWhenLevelUp[i];
                        var currentProbability = (float)(rewardsNumber - selections.Count) / (_survivalModeSettings.PossibleStatModifiersWhenLevelUp.length - i);

                        if (statsComponent.Value.HasStat(statModifier.StatType) && randomValue <= currentProbability)
                        {
                            var sign = statModifier.OperationType == StatModifierOperationType.Multiplication ? "%" : "";
                            var selection = new SelectionData($"Change {statModifier.StatType} to {statModifier.Value}{sign}", entity =>
                            {
                                var enhancement = GetEnhancements(evt.Entity);
                                enhancement.AddModifier(statModifier);
                                
                                _statModificationRequest.Publish(new StatModificationRequest
                                {
                                    Entity = entity, 
                                    statBuff = enhancement
                                }, true);
                            });
                            selections.Add(selection);
                        }
                    }
                }
                else
                {
                    var abilitySelectionsEntity = _abilitySelectionsFilter.FirstOrDefault();
                    ref var abilitySelectionsComponent = ref abilitySelectionsEntity.GetComponent<AbilitySelectionsComponent>();

                    foreach (var data in abilitySelectionsComponent.Value)
                    {
                       selections.Add(data); 
                    }
                }
                
                var selectionGroup = new SelectionGroup(selections, evt.Entity);
                _selector.AddToQueue(selectionGroup);
            }
        }

        public LeveledUpUnitEnhancement GetEnhancements(Entity entity)
        {
            if (!_enhancements.ContainsKey(entity))
                _enhancements.Add(entity, new LeveledUpUnitEnhancement());

            return _enhancements[entity];
        }
    }
}