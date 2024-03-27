using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class AttackTargetFollowingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<MovementComponent>().With<NavMeshAgentComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<AliveComponent>()
                .With<AIComponent>()
                .ForEach((Entity entity, ref NavMeshAgentComponent navMeshAgentComponent, ref AttackTargetComponent targetComponent) =>
                {
                    if (targetComponent.TargetEntity.IsNullOrDisposed()) return;
                    
                    if (navMeshAgentComponent.Value.enabled)
                    {
                        navMeshAgentComponent.Value.isStopped = targetComponent.IsInAttackRadius;
                        
                        if (targetComponent.IsInAttackRadius)
                        {
                            navMeshAgentComponent.Value.velocity = Vector3.zero;
                        }
                    }
                    
                    var targetTransform = targetComponent.TargetEntity.GetComponent<TransformComponent>().Value;
                    var destinationComponent = entity.GetComponent<DestinationComponent>(out var hasDestinationComponent);
                    
                    if (targetComponent.IsInAttackRadius || (hasDestinationComponent && destinationComponent.Value == targetTransform.position)) return;
                    
                    World.GetRequest<DestinationChangeRequest>().Publish(new DestinationChangeRequest
                    {
                        Entity = entity,
                        Destination = targetTransform.position
                    });
                });
        }
    }
}