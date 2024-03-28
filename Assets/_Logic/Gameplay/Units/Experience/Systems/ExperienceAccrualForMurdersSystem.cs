using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using _Logic.Gameplay.Units.Health.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExperienceAccrualForMurdersSystem : AbstractUpdateSystem
    {
        private Request<ExperienceAmountChangeRequest> _experienceAmountChangeRequest;
        private Event<UnitDeathEvent> _unitDeathEvent;
        
        [Inject]
        private ExperienceSettings _experienceSettings;
        
        public override void OnAwake()
        {
            _experienceAmountChangeRequest = World.GetRequest<ExperienceAmountChangeRequest>();
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _unitDeathEvent.publishedChanges)
            {
                if (evt.CorpseEntity.IsNullOrDisposed() || evt.MurdererEntity.IsNullOrDisposed()) continue;

                var experienceComponent = evt.CorpseEntity.GetComponent<ExperienceComponent>(out var hasExperienceComponent);
                var unitExperience = hasExperienceComponent ? experienceComponent.TotalExperienceAmount : 0;
                var experience = _experienceSettings.CalculateExperienceRewardForMurder(unitExperience);
                
                _experienceAmountChangeRequest.Publish(new ExperienceAmountChangeRequest
                {
                    ReceivingEntity = evt.MurdererEntity,
                    Change = experience
                });
            }
        }
    }
}