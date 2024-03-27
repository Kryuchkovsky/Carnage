using System.Collections.Generic;
using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.RewardSelector;
using _Logic.Gameplay.SurvivalMode;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Experience.Events;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Stats.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LeveledUpUnitEnhancementSystem : AbstractUpdateSystem
    {
        private Event<LevelChangeEvent> _levelChangeEvent;
        private Request<StatChangeRequest> _statChangeRequest;
        private ExperienceSettings _experienceSettings;
        private SurvivalModeSettings _survivalModeSettings;

        public override void OnAwake()
        {
            _levelChangeEvent = World.GetEvent<LevelChangeEvent>();
            _statChangeRequest = World.GetRequest<StatChangeRequest>();
            _experienceSettings = ConfigManager.Instance.GetConfig<ExperienceSettings>();
            _survivalModeSettings = ConfigManager.Instance.GetConfig<SurvivalModeSettings>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var levelChangeEvent in _levelChangeEvent.publishedChanges)
            {
                if (levelChangeEvent.Entity.IsNullOrDisposed() ||
                    levelChangeEvent.Change <= 0 ||
                    levelChangeEvent.Entity.Has<AIComponent>() || 
                    !levelChangeEvent.Entity.Has<StatsComponent>()) continue;
                
                ref var statsComponent = ref levelChangeEvent.Entity.GetComponent<StatsComponent>();
                var selections = new List<Selection>(_survivalModeSettings.RewardsNumberWhenLevelUp);
                var rewardsNumber = Mathf.Clamp(
                    _survivalModeSettings.RewardsNumberWhenLevelUp, 0, _survivalModeSettings.PossibleAffectionsWhenLevelUp.Count);
                
                for (int i = 0; i < _survivalModeSettings.PossibleAffectionsWhenLevelUp.Count && selections.Count < rewardsNumber; i++)
                {
                    var randomValue = Random.Range(0f, 1f);
                    var affection = _survivalModeSettings.PossibleAffectionsWhenLevelUp[i];
                    var currentProbability = (float)(rewardsNumber - selections.Count) / (_survivalModeSettings.PossibleAffectionsWhenLevelUp.Count - i);

                    if (statsComponent.Value.HasStat(affection.StatType) && randomValue <= currentProbability)
                    {
                        var sign = affection.OperationType == StatModifierOperationType.Multiplication ? "%" : "";
                        var selection = new Selection($"Change {affection.StatType} to {affection.Value}{sign}", () =>
                        {
                            _statChangeRequest.Publish(new StatChangeRequest
                            {
                                Entity = levelChangeEvent.Entity,
                                Modifier = new StatModifier(affection.OperationType, affection.Value),
                                Type = affection.StatType
                            }, true);
                        });
                        selections.Add(selection);
                    }
                }
                
                var selectionGroup = new SelectionGroup(selections);
                RewardSelectorView.Instance.AddToQueue(selectionGroup);
            }
        }
    }
}