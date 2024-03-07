using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Projectiles;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class RangeAttackHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<RangeAttackComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref AttackComponent attackComponent, 
                    ref AttackTargetComponent attackTargetComponent, ref RangeAttackComponent rangeAttackComponent) =>
                {
                    if (attackComponent.AttackTimePercentage < 1 || !attackTargetComponent.IsInAttackRadius) return;
                    
                    var damage = attackComponent.Stats.Damage;
                    var targetEntity = attackTargetComponent.TargetEntity;
                    var offset = Vector3.zero;
                    var boundsComponent = attackTargetComponent.TargetEntity.GetComponent<BoundsComponent>(out var hasBoundsComponent);

                    if (hasBoundsComponent)
                    {
                        offset += Vector3.up * boundsComponent.Value.extents.y;
                    }
                    
                    World.GetRequest<HomingProjectileCreationRequest>().Publish(new HomingProjectileCreationRequest
                    {
                        Data = attackComponent.Stats.ProjectileData,
                        Target = targetEntity.GetComponent<TransformComponent>().Value,
                        InitialPosition = unitComponent.Value.Model.AttackPoint.position,
                        Offset = offset,
                        Callback = p =>
                        {
                            if (!targetEntity.IsNullOrDisposed())
                            {
                                // if (provider.Entity.TryGetComponentValue<RigidbodyComponent>(out var enemyRigidbodyComponent))
                                // {
                                //     var direction = (enemyRigidbodyComponent.Value.position - transform.position).normalized;
                                //     var force = direction * 2;
                                //     enemyRigidbodyComponent.Value.AddForce(force, ForceMode.VelocityChange);
                                // }
                                        
                                World.GetRequest<DamageRequest>().Publish(new DamageRequest
                                {
                                    AttackerEntity = entity,
                                    ReceiverEntity = targetEntity,
                                    Damage = damage.CurrentValue
                                });
                            }
                        }
                    });

                    attackComponent.AttackTimePercentage = 0;
                    unitComponent.Value.OnAttack();
                });
        }
    }
}