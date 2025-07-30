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
        private Request<StatChangeRequest> _statChangeRequest;
        private Filter _abilitySelectionsFilter;
        private readonly int _levelIntervalBetweenAbilitySelection = 3;

        [Inject] private Selector _selector;
        [Inject] private SurvivalModeSettings _survivalModeSettings;

        public override void OnAwake()
        {
            _levelChangeEvent = World.GetEvent<LevelChangeEvent>();
            _statChangeRequest = World.GetRequest<StatChangeRequest>();
            _abilitySelectionsFilter = World.Filter.With<AbilitySelectionsComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _levelChangeEvent.publishedChanges)
            {
                if (evt.Entity.IsNullOrDisposed() || evt.Change <= 0 || evt.NewLevel <= 1 || evt.Entity.Has<AIComponent>() || !evt.Entity.Has<StatsComponent>()) continue;
                
                ref var statsComponent = ref evt.Entity.GetComponent<StatsComponent>();
                var selections = new List<SelectionData>(_survivalModeSettings.RewardsNumberWhenLevelUp);
                var rewardsNumber = Mathf.Clamp(
                    _survivalModeSettings.RewardsNumberWhenLevelUp, 0, _survivalModeSettings.PossibleAffectionsWhenLevelUp.Count);

                var hasSelectionOfAbilities = evt.NewLevel % _levelIntervalBetweenAbilitySelection == 0;

                if (hasSelectionOfAbilities)
                {
                    for (int i = 0; i < _survivalModeSettings.PossibleAffectionsWhenLevelUp.Count && selections.Count < rewardsNumber; i++)
                    {
                        var randomValue = Random.Range(0f, 1f);
                        var affection = _survivalModeSettings.PossibleAffectionsWhenLevelUp[i];
                        var currentProbability = (float)(rewardsNumber - selections.Count) / (_survivalModeSettings.PossibleAffectionsWhenLevelUp.Count - i);

                        if (statsComponent.Value.HasStat(affection.StatType) && randomValue <= currentProbability)
                        {
                            var sign = affection.OperationType == StatModifierOperationType.Multiplication ? "%" : "";
                            var selection = new SelectionData($"Change {affection.StatType} to {affection.Value}{sign}", entity =>
                            {
                                _statChangeRequest.Publish(new StatChangeRequest
                                {
                                    Entity = entity,
                                    Modifier = new StatModifier(affection.OperationType, affection.Value),
                                    Type = affection.StatType
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
    }
}