using _Logic.Core.Components;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class AttackHandlingSystem : QuerySystem
    {
        private readonly Collider[] _colliders;
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<TeamIdComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref AttackComponent attackComponent, 
                    ref TeamIdComponent teamIdComponent, ref TransformComponent transformComponent) =>
                {
                    if (attackComponent.AttackTimePercentage < 1) return;
                    
                    var collisions = Physics.OverlapSphereNonAlloc(
                        transformComponent.Value.position, attackComponent.CurrentData.Range, _colliders);

                    if (collisions == 0) return;

                    var isAttacking = false;
                    
                    for (int i = 0; i < _colliders.Length; i++)
                    {
                        if (_colliders[i].TryGetComponent(out UnitProvider provider) &&
                            provider.Entity.TryGetComponent<TeamIdComponent>(out var enemyTeamIdComponent) &&
                            enemyTeamIdComponent.Value != teamIdComponent.Value)
                        {
                            var direction = (_colliders[i].transform.position -
                                             transformComponent.Value.position).normalized;

                            var angle = Vector3.Angle(transformComponent.Value.forward, direction);

                            if (angle > 60) return;
                            
                            World.GetRequest<DamageRequest>().Publish(new DamageRequest
                            {
                                ReceiverEntity = provider.Entity,
                                Damage = attackComponent.CurrentData.Damage
                            });

                            isAttacking = true;
                        }
                    }

                    if (isAttacking)
                    {
                        attackComponent.AttackTimePercentage = 0;
                        unitComponent.Value.PlayAttackAnimation();
                    }
                });
        }
    }
}