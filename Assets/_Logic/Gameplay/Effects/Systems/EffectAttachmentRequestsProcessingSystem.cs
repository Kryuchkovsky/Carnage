using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.VFXManager;
using _Logic.Gameplay.Effects.Requests;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Requests;
using _Logic.Gameplay.Units.Stats.Requests;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Effects.Systems
{
    public class EffectAttachmentRequestsProcessingSystem : AbstractUpdateSystem
    {
        private Request<EffectAttachmentRequest> _effectAttachmentRequest;
        private Request<StatModificationRequest> _statModificationRequest;
        private Request<HealthChangeProcessAdditionRequest> _healthChangeProcessAdditionRequest;
        
        [Inject] private GameEffectCatalog _gameEffectCatalog;
        [Inject] private VFXService _vfxService;

        public override void OnAwake()
        {
            _effectAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
            _statModificationRequest = World.GetRequest<StatModificationRequest>();
            _healthChangeProcessAdditionRequest = World.GetRequest<HealthChangeProcessAdditionRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _effectAttachmentRequest.Consume())
            {
                if (request.TargetEntity.IsNullOrDisposed() || request.EffectType == EffectType.None) 
                    continue;
                
                var data = _gameEffectCatalog.GetData(request.EffectType);

                if (data.IsChangingHealth && request.TargetEntity.Has<HealthComponent>())
                {
                    var healthChangeProcess = new HealthChangeProcess(
                        data.HealthChangeData, 
                        data.HealthChangeOperationType, 
                        data.Duration,
                        data.HealthChangeInterval);
                    
                    _healthChangeProcessAdditionRequest.Publish(new HealthChangeProcessAdditionRequest
                    {
                        TargetEntity = request.TargetEntity,
                        Process = healthChangeProcess
                    });
                }

                _statModificationRequest.Publish(new StatModificationRequest
                {
                    Entity = request.TargetEntity, 
                    statBuff = data
                });

                ref var transformComponent = ref request.TargetEntity.GetComponent<TransformComponent>(out var hasTransformComponent);

                if (hasTransformComponent)
                {
                    _vfxService.CreateEffect(data.VFXType, transformComponent.Value.position, Quaternion.identity, transformComponent.Value, data.Duration);
                }
            }
        }
    }
}