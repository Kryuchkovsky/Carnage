using _Logic.Core;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExperienceAmountChangeRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<ExperienceAmountChangeRequest> _experienceAmountChangeRequest;
        private Request<LevelChangeRequest> _levelChangeRequest;
        
        [Inject] private ExperienceSettings _experienceSettings;
        
        public override void OnAwake()
        {
            _experienceAmountChangeRequest = World.GetRequest<ExperienceAmountChangeRequest>();
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _experienceAmountChangeRequest.Consume())
            {
                if (request.ReceivingEntity.IsNullOrDisposed() || !request.ReceivingEntity.Has<ExperienceComponent>()) continue;
                
                ref var expComponent = ref request.ReceivingEntity.GetComponent<ExperienceComponent>();
                expComponent.TotalExperienceAmount += request.Change;
                expComponent.Progress = (expComponent.TotalExperienceAmount - expComponent.CurrentLevelCost) / expComponent.LevelUpCost;
                var newLevelCost = expComponent.NextLevelCost;
                var levelChange = 0;

                while (expComponent.TotalExperienceAmount > newLevelCost)
                {
                    levelChange += 1;
                    newLevelCost += _experienceSettings.CalculateLevelCost(expComponent.Level + levelChange);
                }

                if (levelChange != 0)
                {
                    _levelChangeRequest.Publish(new LevelChangeRequest
                    {
                        Entity = request.ReceivingEntity,
                        Change = levelChange
                    });
                }
                else
                {
                    ref var expBarComponent = ref request.ReceivingEntity.GetComponent<ExperienceBarComponent>(out var hasExperienceBarComponent);
                
                    if (hasExperienceBarComponent)
                    {
                        expBarComponent.Value.SetLevel(expComponent.Level);
                        expBarComponent.Value.SetExperienceBarFilling(expComponent.Progress);
                    }
                }
            }
        }
    }
}