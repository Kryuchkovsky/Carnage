using _Logic.Core.Components;
using _Logic.Gameplay.Units.Projectiles.Components;
using _Logic.Gameplay.Units.Projectiles.Events;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Projectiles.Systems
{
    public sealed class ProjectileFlightProcessingSystem : QuerySystem
    {
        private Event<ProjectileFlightEndEvent> _projectileFlightEndEvent;
        
        public override void OnAwake()
        {
            base.OnAwake();
            _projectileFlightEndEvent = World.GetEvent<ProjectileFlightEndEvent>();
        }

        protected override void Configure()
        {
            CreateQuery()
                .With<ProjectileComponent>().With<ProjectileDataComponent>().With<TransformComponent>().With<OwnerComponent>()
                .ForEach((Entity entity, ref ProjectileComponent projectileComponent, ref ProjectileDataComponent projectileDataComponent, ref TransformComponent transformComponent, ref OwnerComponent ownerComponent) =>
                {
                    ref var targetComponent = ref entity.GetComponent<TargetComponent>(out var hasTargetComponent);
                
                    if (hasTargetComponent && !   targetComponent.Entity.IsNullOrDisposed())
                    {
                        ref var targetTransformComponent = ref targetComponent.Entity.GetComponent<TransformComponent>(out var targetHasTransformComponent);
                        
                        if (targetHasTransformComponent)
                        {
                            var targetPosition = targetTransformComponent.Value.position;
                            var targetBoundsComponent = targetComponent.Entity.GetComponent<BoundsComponent>(out var targetHasBoundsComponent);

                            if (targetHasBoundsComponent)
                            {
                                targetPosition += Vector3.up * targetBoundsComponent.Value.extents.y;
                            }
                        
                            entity.SetComponent(new DestinationComponent
                            {
                                Value = targetPosition
                            });
                        }
                    }

                    ref var destinationComponent = ref entity.GetComponent<DestinationComponent>(out var hasDestinationComponent);
                    
                    if (!hasDestinationComponent) return;
                    
                    var projectilePosition = transformComponent.Value.position;
                    var direction = destinationComponent.Value - projectilePosition;
                    var distanceWithoutHeight = new Vector3(direction.x, 0, direction.z).magnitude;
                    var destination = destinationComponent.Value + Vector3.up * distanceWithoutHeight * projectileDataComponent.Value.FlightRangeToHeightRatio;
                    var delta = (destination - projectilePosition).normalized * projectileDataComponent.Value.Speed * deltaTime;
                    transformComponent.Value.rotation = Quaternion.LookRotation(delta);

                    if (delta.magnitude >= direction.magnitude)
                    {
                        _projectileFlightEndEvent.NextFrame(new ProjectileFlightEndEvent
                        {
                            OwnerEntity = ownerComponent.Entity,
                            TargetEntity = hasTargetComponent ? targetComponent.Entity : null,
                            ProjectileEntity = entity
                        });

                        if (hasTargetComponent)
                        {
                            entity.RemoveComponent<TargetComponent>();
                        }
                        
                        entity.RemoveComponent<DestinationComponent>();
                    }
                    else
                    {
                        transformComponent.Value.position += delta;
                    }
                });
        }
    }
}