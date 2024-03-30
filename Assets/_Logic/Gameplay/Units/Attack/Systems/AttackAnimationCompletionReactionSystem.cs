using _Logic.Core;
using _Logic.Gameplay.Projectiles.Events;
using _Logic.Gameplay.Projectiles.Requests;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Requests;
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
            foreach (var @event in _attackAnimationCompletionEvent.publishedChanges)
            {
                if (@event.Entity.IsNullOrDisposed()) continue;

                var entity = @event.Entity;
                ref var unit = ref entity.GetComponent<UnitComponent>(out var hasUnit);
                ref var attack = ref entity.GetComponent<AttackComponent>(out var hasAttack);
                ref var attackTarget = ref entity.GetComponent<AttackTargetComponent>(out var hasAttackTarget);
                ref var stats = ref entity.GetComponent<StatsComponent>(out var hasStats);

                if (!hasUnit || !hasAttack || !hasAttackTarget || !hasStats || attackTarget.TargetEntity.IsNullOrDisposed()) continue;

                var targetEntity = attackTarget.TargetEntity;
                
                _attackStartEvent.NextFrame(new AttackStartEvent
                {
                    AttackingEntity = entity,
                    AttackedEntity = targetEntity
                });
                
                if (attack.ProjectileType == ProjectileType.None)
                {
                    var damage = stats.Value.GetCurrentValue(StatType.AttackDamage);
                    
                    var healthChangeData = new HealthChangeData
                    {
                        Type = attack.AttackHealthChangeType,
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
                        Type = attack.ProjectileType,
                        InitialPosition = unit.Value.Model.AttackPoint.position,
                    }, true);
                }
            }

            foreach (var @event in _projectileFlightEndEvent.publishedChanges)
            {
                if (@event.OwnerEntity.IsNullOrDisposed() || !@event.OwnerEntity.Has<AttackComponent>() || !@event.OwnerEntity.Has<StatsComponent>()) continue;
                
                ref var attackComponent = ref @event.OwnerEntity.GetComponent<AttackComponent>();
                ref var statsComponent = ref @event.OwnerEntity.GetComponent<StatsComponent>();
                
                var damage = statsComponent.Value.GetCurrentValue(StatType.AttackDamage);
                
                var healthChangeData = new HealthChangeData
                {
                    Type = attackComponent.AttackHealthChangeType,
                    Value = damage
                };
                var healthChangeRequest = new HealthChangeRequest
                {
                    TargetEntity = @event.TargetEntity,
                    SenderEntity = @event.OwnerEntity,
                    Data = healthChangeData,
                    CreatePopup = true
                };
                _healthChangeRequest.Publish(healthChangeRequest);

                if (@event.OwnerEntity.Has<UnitComponent>())
                {
                    _attackEndEvent.NextFrame(new AttackEndEvent
                    {
                        AttackingEntity = @event.OwnerEntity,
                        AttackedEntity = @event.TargetEntity
                    });
                }
            }
        }
    }
}