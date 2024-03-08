using System;
using _Logic.Core;
using _Logic.Extensions.VFXManager;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class DamageRequestsProcessingSystem : AbstractSystem
    {
        private Request<DamageRequest> _damageRequest;
        private Request<HealthChangeRequest> _healthChangeRequest;
        private readonly float _effectIndent = 0.5f;

        public override void OnAwake()
        {
            _damageRequest = World.GetRequest<DamageRequest>();
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _damageRequest.Consume())   
            {
                if (request.AttackerEntity.IsNullOrDisposed() || 
                    request.ReceiverEntity.IsNullOrDisposed() ||
                    !request.AttackerEntity.Has<UnitComponent>() ||
                    !request.ReceiverEntity.Has<UnitComponent>()) continue;
                
                _healthChangeRequest.Publish(new HealthChangeRequest
                {
                    Entity = request.ReceiverEntity,
                    Change = -request.Damage
                });
                
                var attackingUnit = request.AttackerEntity.GetComponent<UnitComponent>().Value;
                var receivingUnit = request.ReceiverEntity.GetComponent<UnitComponent>().Value;
                var receiverPosition = receivingUnit.transform.position;
                var effectPosition = receiverPosition + (attackingUnit.transform.position - receiverPosition).normalized * _effectIndent;
                EffectsService.Instance.CreateEffect(receivingUnit.Model.HitEffectType, effectPosition);
            }
        }
    }
}