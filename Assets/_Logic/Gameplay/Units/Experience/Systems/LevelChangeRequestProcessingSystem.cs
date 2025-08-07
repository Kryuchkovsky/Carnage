using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.VFXManager;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Events;
using _Logic.Gameplay.Units.Experience.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LevelChangeRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<LevelChangeRequest> _levelChangeRequest;
        private Event<LevelChangeEvent> _levelChangeEvent;
        
        [Inject] private ExperienceSettings _experienceSettings;
        [Inject] private VFXService _vfxService;
        
        public override void OnAwake()
        {
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
            _levelChangeEvent = World.GetEvent<LevelChangeEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _levelChangeRequest.Consume())
            {
                if (request.Entity.IsNullOrDisposed()) 
                    continue;
                
                ref var expComponent = ref request.Entity.GetComponent<ExperienceComponent>(out var hasExpComponent);
                
                if (hasExpComponent)
                {
                    expComponent.Level = Mathf.Clamp(expComponent.Level + request.Change, 1, int.MaxValue);
                }
                else
                {
                    var newLevel = Mathf.Clamp(request.Change, 1, int.MaxValue);
                    
                    expComponent = new ExperienceComponent
                    {
                        Level = newLevel
                    };
                }
                
                _levelChangeEvent.NextFrame(new LevelChangeEvent
                {
                    Entity = request.Entity,
                    NewLevel = expComponent.Level,
                    Change = request.Change
                });

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

                ref var transformComponent = ref request.Entity.GetComponent<TransformComponent>(out var hasTransformComponent);

                if (hasExpComponent && hasTransformComponent)
                    _vfxService.CreateEffect(_experienceSettings.LevelUpVFX, transformComponent.Value.position);

                if (!hasExpComponent)
                    request.Entity.SetComponent(expComponent);
            }
        }
    }
}