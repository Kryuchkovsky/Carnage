using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Items.Requests;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Health.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExperienceDropFromDeadSystem : AbstractUpdateSystem
    {
        private Request<ExperienceEssenceCreationRequest> _experienceEssenceCreationRequest;
        private Event<UnitDeathEvent> _unitDeathEvent;
        
        [Inject] private ExperienceSettings _experienceSettings;
        
        public override void OnAwake()
        {
            _experienceEssenceCreationRequest = World.GetRequest<ExperienceEssenceCreationRequest>();
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _unitDeathEvent.publishedChanges)
            {
                if (!_experienceSettings.ExpDropIsEnabled || evt.CorpseEntity.IsNullOrDisposed() || !evt.CorpseEntity.Has<ExperienceComponent>() || !evt.CorpseEntity.Has<TransformComponent>()) 
                    continue;

                var experienceComponent = evt.CorpseEntity.GetComponent<ExperienceComponent>();
                var transformComponent = evt.CorpseEntity.GetComponent<TransformComponent>();
                var experience = _experienceSettings.CalculateExperienceRewardForMurder(experienceComponent.TotalExperienceAmount);

                _experienceEssenceCreationRequest.Publish(new ExperienceEssenceCreationRequest
                {
                    Position = transformComponent.Value.position + Vector3.up,
                    ExperienceAmount = experience
                });
            }
        }
    }
}