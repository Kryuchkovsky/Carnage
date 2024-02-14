using _Logic.Core.Components;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Projectiles;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class RangeAttackHandlingSystem : QuerySystem
    {
        private Collider[] _colliders = new Collider[10];
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<RangeAttackComponent>().With<TeamIdComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref AttackComponent attackComponent, 
                    ref RangeAttackComponent rangeAttackComponent, ref TeamIdComponent teamIdComponent, ref TransformComponent transformComponent) =>
                {
                    if (attackComponent.AttackTimePercentage < 1) return;

                    var collisions = Physics.OverlapSphereNonAlloc(transformComponent.Value.position, attackComponent.CurrentData.Range, _colliders);
                    var transform = transformComponent.Value;
                    
                    var isAttacking = false;
                    
                    for (int i = 0; i < collisions; i++)
                    {
                        if (_colliders[i].TryGetComponent(out UnitProvider provider) &&
                            !provider.Entity.IsNullOrDisposed() &&
                            provider.Entity.TryGetComponentValue<TeamIdComponent>(out var enemyTeamIdComponent) &&
                            enemyTeamIdComponent.Value != teamIdComponent.Value)
                        {
                            var damage = attackComponent.CurrentData.Damage;
                            World.GetRequest<HomingProjectileCreationRequest>().Publish(new HomingProjectileCreationRequest
                            {
                                Data = attackComponent.CurrentData.ProjectileData,
                                Target = provider.transform,
                                InitialPosition = unitComponent.Value.ProjectileSpawnPoint.position,
                                Callback = p =>
                                {
                                    if (provider && !provider.Entity.IsNullOrDisposed())
                                    {
                                        if (provider.Entity.TryGetComponentValue<RigidbodyComponent>(out var enemyRigidbodyComponent))
                                        {
                                            var direction = (_colliders[i].transform.position - transform.position).normalized;
                                            var force = direction * 2;
                                            enemyRigidbodyComponent.Value.AddForce(force, ForceMode.VelocityChange);
                                        }
                                        
                                        World.GetRequest<DamageRequest>().Publish(new DamageRequest
                                        {
                                            AttackerEntity = entity,
                                            ReceiverEntity = provider.Entity,
                                            Damage = damage
                                        });
                                    }
                                }
                            });
                            
                            isAttacking = true;
                            break;
                        }
                    }

                    if (isAttacking)
                    {
                        attackComponent.AttackTimePercentage = 0;
                        unitComponent.Value.OnAttack();
                    }
                });
        }
    }
}