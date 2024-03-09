using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Experience.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LevelChangeRequestProcessingSystem : AbstractSystem
    {
        private Request<LevelChangeRequest> _request;
        private ExperienceSettings _experienceSettings;
        
        public override void OnAwake()
        {
            _request = World.GetRequest<LevelChangeRequest>();
            _experienceSettings = ConfigsManager.GetConfig<ExperienceSettings>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (request.Entity.Has<ExperienceComponent>())
                {
                    ref var expComponent = ref request.Entity.GetComponent<ExperienceComponent>();
                    expComponent.Level = Mathf.Clamp(expComponent.Level + request.Change, 1, int.MaxValue);
                    var requiredExperienceAmount = _experienceSettings.CalculateRequiredExperienceAmountForLevel(expComponent.Level);
                    var currentExperienceAmount = requiredExperienceAmount > expComponent.CurrentExperienceAmount
                        ? requiredExperienceAmount
                        : expComponent.CurrentExperienceAmount;
                    expComponent.CurrentExperienceAmount = currentExperienceAmount;
                    expComponent.ExperienceAmountForNextLevel = _experienceSettings.CalculateRequiredExperienceAmountForLevel(expComponent.Level + 1);
                }
                else
                {
                    var level = Mathf.Clamp(request.Change, 1, int.MaxValue);
                    request.Entity.SetComponent(new ExperienceComponent
                    {
                        Level = level,
                        CurrentExperienceAmount = _experienceSettings.CalculateRequiredExperienceAmountForLevel(level),
                        ExperienceAmountForNextLevel = _experienceSettings.CalculateRequiredExperienceAmountForLevel(level + 1)
                    });
                }
            }
        }
    }
}