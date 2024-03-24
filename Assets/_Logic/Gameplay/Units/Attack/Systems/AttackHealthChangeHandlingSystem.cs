using _Logic.Core;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Health;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackHealthChangeHandlingSystem : AbstractUpdateSystem
    {
        private Request<HealthChangeRequest> _healthChangeRequest;
        private Event<AttackEndEvent> _attackEndEvent;

        public override void OnAwake()
        {
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
            _attackEndEvent = World.GetEvent<AttackEndEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var attackEndEvent in _attackEndEvent.publishedChanges)
            {
                if (attackEndEvent.AttackingEntity.IsNullOrDisposed()) continue;
                
                ref var attackComponent = ref attackEndEvent.AttackingEntity.GetComponent<AttackComponent>(out var hasAttackComponent);

                if (!hasAttackComponent) continue;
                
                var healthChangeData = new HealthChangeData
                {
                    Type = attackComponent.Stats.HealthChangeType,
                    Value = attackComponent.Stats.Damage.CurrentValue
                };
                var healthChangeRequest = new HealthChangeRequest
                {
                    TargetEntity = attackEndEvent.AttackedEntity,
                    SenderEntity = attackEndEvent.AttackingEntity,
                    Data = healthChangeData
                };
                _healthChangeRequest.Publish(healthChangeRequest);
            }
        }
    }
}