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
        private readonly float _effectIndent = 0.5f;

        public override void OnAwake()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in World.GetRequest<DamageRequest>().Consume())   
            {
                World.GetRequest<HealthChangeRequest>().Publish(new HealthChangeRequest
                {
                    Entity = request.ReceiverEntity,
                    Change = -request.Damage
                });

                if (request.AttackerEntity.IsNullOrDisposed() || 
                    request.ReceiverEntity.IsNullOrDisposed() ||
                    request.AttackerEntity.Has<UnitComponent>() ||
                    request.ReceiverEntity.Has<UnitComponent>()) continue;

                var attackingUnit = request.AttackerEntity.GetComponent<UnitComponent>().Value;
                var receivingUnit = request.ReceiverEntity.GetComponent<UnitComponent>().Value;
                
                if (!String.IsNullOrEmpty(receivingUnit.Model.HitEffectId))
                {
                    var receiverPosition = receivingUnit.transform.position;
                    var effectPosition = receiverPosition + (attackingUnit.transform.position - receiverPosition).normalized * _effectIndent;
                    EffectsService.Instance.CreateEffect(receivingUnit.Model.HitEffectId, effectPosition);
                }
            }
        }
    }
}