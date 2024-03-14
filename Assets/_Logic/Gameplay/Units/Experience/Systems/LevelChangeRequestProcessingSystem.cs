using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LevelChangeRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<LevelChangeRequest> _levelChangeRequest;
        private Request<ExperienceAmountChangeRequest> _experienceAmountChangeRequest;
        private ExperienceSettings _experienceSettings;
        
        public override void OnAwake()
        {
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
            _experienceAmountChangeRequest = World.GetRequest<ExperienceAmountChangeRequest>();
            _experienceSettings = ConfigManager.GetConfig<ExperienceSettings>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _levelChangeRequest.Consume())
            {
                if (request.Entity.IsNullOrDisposed()) continue;
                
                ref var expComponent = ref request.Entity.GetComponent<ExperienceComponent>(out var hasExpComponent);
                
                if (hasExpComponent)
                {
                    expComponent.Level = Mathf.Clamp(expComponent.Level + request.Change, 1, int.MaxValue);
                }
                else
                {
                    var level = Mathf.Clamp(request.Change, 1, int.MaxValue);
                    expComponent = new ExperienceComponent
                    {
                        Level = level
                    };
                    request.Entity.SetComponent(expComponent);
                }

                var levelCost = _experienceSettings.CalculateLevelCost(expComponent.Level);
                var currentExperienceAmount = levelCost > expComponent.TotalExperienceAmount
                    ? levelCost
                    : expComponent.TotalExperienceAmount;
                
                var nextLevelCost = _experienceSettings.CalculateLevelCost(expComponent.Level + 1);
                var levelUpCost = nextLevelCost - levelCost;
                expComponent.TotalExperienceAmount = currentExperienceAmount;
                expComponent.CurrentLevelCost = levelCost;
                expComponent.NextLevelCost = nextLevelCost;
                expComponent.LevelUpCost = levelUpCost;
                expComponent.Progress = (currentExperienceAmount - levelCost) / levelUpCost;
                
                ref var expBarComponent = ref request.Entity.GetComponent<ExperienceBarComponent>(out var hasExperienceBarComponent);
                
                if (hasExperienceBarComponent)
                {
                    expBarComponent.Value.SetLevel(expComponent.Level);
                    expBarComponent.Value.SetExperienceBarFilling(expComponent.Progress);
                }
            }
        }
    }
}