using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class CleaveDamageRequestProcessingSystem : QuerySystem
    {
        private Collider[] _colliders = new Collider[10];
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<MeleeAttackComponent>().With<TeamDataComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref AttackComponent attackComponent, 
                    ref TeamDataComponent teamDataComponent, ref TransformComponent transformComponent) =>
                {
                    if (attackComponent.AttackTimePercentage < 1) return;
                    
                    var collisions = Physics.OverlapSphereNonAlloc(
                        transformComponent.Value.position, attackComponent.CurrentData.Range, _colliders, 1 << teamDataComponent.EnemiesLayer);

                    var isAttacking = false;
                    
                    for (int i = 0; i < collisions; i++)
                    {
                        if (_colliders[i].TryGetComponent(out UnitProvider provider) && !provider.Entity.IsNullOrDisposed())
                        {
                            var enemyTeamDataComponent = provider.Entity.GetComponent<TeamDataComponent>(out var hasEnemyTeamDataComponent);
                            
                            if (hasEnemyTeamDataComponent && enemyTeamDataComponent.Id == teamDataComponent.Id) continue;
                            
                            var direction = (_colliders[i].transform.position - transformComponent.Value.position).normalized;
                            var angle = Vector3.Angle(transformComponent.Value.forward, direction);

                            if (angle > 60) return;
                            
                            World.GetRequest<DamageRequest>().Publish(new DamageRequest
                            {
                                AttackerEntity = entity,
                                ReceiverEntity = provider.Entity,
                                Damage = attackComponent.CurrentData.Damage
                            });

                            var rigidbodyComponent = provider.Entity.GetComponent<RigidbodyComponent>(out var hasRigidbodyComponent);
                            
                            if (hasRigidbodyComponent)
                            {
                                var force = direction * 2;
                                rigidbodyComponent.Value.AddForce(force, ForceMode.VelocityChange);
                            }

                            isAttacking = true;
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