using _Logic.Core;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Requests;
using _Logic.Gameplay.Units.Projectiles.Events;
using _Logic.Gameplay.Units.Projectiles.Requests;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackAnimationCompletionReactionSystem : AbstractUpdateSystem
    {
        private Event<AttackAnimationCompletionEvent> _attackAnimationCompletionEvent;
        private Event<AttackStartEvent> _attackStartEvent;
        private Event<AttackEndEvent> _attackEndEvent;
        private Event<ProjectileFlightEndEvent> _projectileFlightEndEvent;
        private Request<ProjectileCreationRequest> _projectileCreationRequest;
        private Request<HealthChangeRequest> _healthChangeRequest;

        public override void OnAwake()
        {
            _attackAnimationCompletionEvent = World.GetEvent<AttackAnimationCompletionEvent>();
            _attackStartEvent = World.GetEvent<AttackStartEvent>();
            _attackEndEvent = World.GetEvent<AttackEndEvent>();
            _projectileFlightEndEvent = World.GetEvent<ProjectileFlightEndEvent>();
            _projectileCreationRequest = World.GetRequest<ProjectileCreationRequest>();
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var completionEvent in _attackAnimationCompletionEvent.publishedChanges)
            {
                if (completionEvent.Entity.IsNullOrDisposed()) continue;

                var entity = completionEvent.Entity;
                ref var unitComponent = ref entity.GetComponent<UnitComponent>(out var hasUnitComponent);
                ref var attackComponent = ref entity.GetComponent<AttackComponent>(out var hasAttackComponent);
                ref var attackTargetComponent = ref entity.GetComponent<AttackTargetComponent>(out var hasAttackTargetComponent);
                ref var statsComponent = ref entity.GetComponent<StatsComponent>(out var hasStatsComponent);

                if (!hasUnitComponent || !hasAttackComponent || !hasAttackTargetComponent || !hasStatsComponent || attackTargetComponent.TargetEntity.IsNullOrDisposed()) continue;

                var targetEntity = attackTargetComponent.TargetEntity;
                
                _attackStartEvent.NextFrame(new AttackStartEvent
                {
                    AttackingEntity = entity,
                    AttackedEntity = targetEntity
                });
                
                if (attackComponent.ProjectileType == ProjectileType.None)
                {
                    statsComponent.Value.TryGetCurrentValue(StatType.AttackDamage, out var damage);
                    
                    var healthChangeData = new HealthChangeData
                    {
                        Type = attackComponent.AttackHealthChangeType,
                        Value = damage
                    };
                    var healthChangeRequest = new HealthChangeRequest
                    {
                        TargetEntity = targetEntity,
                        SenderEntity = entity,
                        Data = healthChangeData
                    };
                    _healthChangeRequest.Publish(healthChangeRequest);
                    
                    _attackEndEvent.NextFrame(new AttackEndEvent
                    {
                        AttackingEntity = entity,
                        AttackedEntity = targetEntity
                    });
                }
                else
                {
                    _projectileCreationRequest.Publish(new ProjectileCreationRequest
                    {
                        OwnerEntity = entity,
                        TargetEntity = targetEntity,
                        Type = attackComponent.ProjectileType,
                        InitialPosition = unitComponent.Value.Model.AttackPoint.position,
                    }, true);
                }
            }

            foreach (var endEvent in _projectileFlightEndEvent.publishedChanges)
            {
                if (endEvent.OwnerEntity.IsNullOrDisposed() || !endEvent.OwnerEntity.Has<AttackComponent>() || !endEvent.OwnerEntity.Has<StatsComponent>()) continue;
                
                ref var attackComponent = ref endEvent.OwnerEntity.GetComponent<AttackComponent>();
                ref var statsComponent = ref endEvent.OwnerEntity.GetComponent<StatsComponent>();
                
                statsComponent.Value.TryGetCurrentValue(StatType.AttackDamage, out var damage);
                
                var healthChangeData = new HealthChangeData
                {
                    Type = attackComponent.AttackHealthChangeType,
                    Value = damage
                };
                var healthChangeRequest = new HealthChangeRequest
                {
                    TargetEntity = endEvent.TargetEntity,
                    SenderEntity = endEvent.OwnerEntity,
                    Data = healthChangeData,
                    CreatePopup = true
                };
                _healthChangeRequest.Publish(healthChangeRequest);

                if (endEvent.OwnerEntity.Has<UnitComponent>())
                {
                    _attackEndEvent.NextFrame(new AttackEndEvent
                    {
                        AttackingEntity = endEvent.OwnerEntity,
                        AttackedEntity = endEvent.TargetEntity
                    });
                }
            }
        }
    }
}