using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Experience.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExperienceAmountChangeRequestProcessingSystem : AbstractSystem
    {
        private Request<ExperienceAmountChangeRequest> _experienceAmountChangeRequest;
        private Request<LevelChangeRequest> _levelChangeRequest;
        private ExperienceSettings _experienceSettings;
        
        public override void OnAwake()
        {
            _experienceAmountChangeRequest = World.GetRequest<ExperienceAmountChangeRequest>();
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
            _experienceSettings = ConfigsManager.GetConfig<ExperienceSettings>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _experienceAmountChangeRequest.Consume())
            {
                if (request.ReceivingEntity.Has<ExperienceComponent>())
                {
                    ref var experienceComponent = ref request.ReceivingEntity.GetComponent<ExperienceComponent>();
                    experienceComponent.CurrentExperienceAmount += request.Change;
                    var newLevel = _experienceSettings.GetLevelByExperienceAmount(experienceComponent.CurrentExperienceAmount);
                    var levelChange = newLevel - experienceComponent.Level;

                    if (levelChange != 0)
                    {
                        _levelChangeRequest.Publish(new LevelChangeRequest
                        {
                            Entity = request.ReceivingEntity,
                            Change = levelChange
                        });
                    }
                }
            }
        }
    }
}