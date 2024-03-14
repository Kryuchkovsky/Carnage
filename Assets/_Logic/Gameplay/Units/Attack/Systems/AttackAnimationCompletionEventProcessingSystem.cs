using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Effects.Requests;
using _Logic.Gameplay.Units.Projectiles;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class AttackAnimationCompletionEventProcessingSystem : AbstractUpdateSystem
    {
        private Event<AttackAnimationCompletionEvent> _event;
        private Request<DamageRequest> _damageRequest;
        private Request<HomingProjectileCreationRequest> _projectileCreationRequest;
        private Request<ImpactCreationRequest> _impactCreationRequest;

        public override void OnAwake()
        {
            _event = World.GetEvent<AttackAnimationCompletionEvent>();
            _damageRequest = World.GetRequest<DamageRequest>();
            _projectileCreationRequest = World.GetRequest<HomingProjectileCreationRequest>();
            _impactCreationRequest = World.GetRequest<ImpactCreationRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var completionEvent in _event.publishedChanges)
            {
                if (completionEvent.Entity.IsNullOrDisposed()) continue;

                ref var unitComponent = ref completionEvent.Entity.GetComponent<UnitComponent>(out var hasUnitComponent);
                ref var attackComponent = ref completionEvent.Entity.GetComponent<AttackComponent>(out var hasAttackComponent);
                ref var attackTargetComponent = ref completionEvent.Entity.GetComponent<AttackTargetComponent>(out var hasAttackTargetComponent);

                if (hasUnitComponent && hasAttackComponent && hasAttackTargetComponent && !attackTargetComponent.TargetEntity.IsNullOrDisposed())
                {
                    var attackerEntity = completionEvent.Entity;
                    var targetEntity = attackTargetComponent.TargetEntity;
                    var damage = attackComponent.Stats.Damage.CurrentValue;
                    var targetTransformComponent = targetEntity.GetComponent<TransformComponent>(out var targetHasTransformComponent);
                    
                    if (attackComponent.Stats.ProjectileData && targetHasTransformComponent && targetTransformComponent.Value)
                    {
                        var offset = Vector3.zero;
                        var boundsComponent = attackTargetComponent.TargetEntity.GetComponent<BoundsComponent>(out var hasBoundsComponent);

                        if (hasBoundsComponent)
                        {
                            offset += Vector3.up * boundsComponent.Value.extents.y;
                        }
                    
                        _projectileCreationRequest.Publish(new HomingProjectileCreationRequest
                        {
                            Data = attackComponent.Stats.ProjectileData,
                            Target = targetTransformComponent.Value,
                            InitialPosition = unitComponent.Value.Model.AttackPoint.position,
                            Offset = offset,
                            Callback = p =>
                            {
                                if (!targetEntity.IsNullOrDisposed())
                                {
                                    _damageRequest.Publish(new DamageRequest
                                    {
                                        AttackerEntity = attackerEntity,
                                        ReceiverEntity = targetEntity,
                                        Damage = damage
                                    });
                                }
                            }
                        }, true);
                    }
                    else
                    {
                        _damageRequest.Publish(new DamageRequest
                        {
                            AttackerEntity = attackerEntity,
                            ReceiverEntity = targetEntity,
                            Damage = damage
                        });
                    }
                }
            }
        }
    }
}