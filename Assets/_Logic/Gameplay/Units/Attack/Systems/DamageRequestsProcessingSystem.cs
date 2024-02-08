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

                if (request.AttackerEntity.TryGetComponentValue<UnitComponent>(out var attackingUnit) &&
                    request.ReceiverEntity.TryGetComponentValue<UnitComponent>(out var receivingUnit))
                {
                    var receiverPosition = receivingUnit.Value.transform.position;
                    var effectPosition = receiverPosition + (attackingUnit.Value.transform.position - receiverPosition).normalized * _effectIndent;
                    EffectCreationService.Instance.CreateEffect(receivingUnit.Value.Model.HitEffectId, effectPosition);
                }
            }
        }
    }
}