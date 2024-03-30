using _Logic.Core.Components;
using _Logic.Gameplay.Projectiles.Components;
using _Logic.Gameplay.Projectiles.Events;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Projectiles.Systems
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
                .With<FlightParametersComponent>().With<DestinationComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref FlightParametersComponent parametersComponent, ref DestinationComponent destinationComponent, ref TransformComponent transformComponent) =>
                {

                    var projectilePosition = transformComponent.Value.position;
                    var direction = destinationComponent.Value - projectilePosition;
                    var distanceWithoutHeight = new Vector3(direction.x, 0, direction.z).magnitude;
                    var destination = destinationComponent.Value + Vector3.up * distanceWithoutHeight * parametersComponent.FlightRangeToHeightRatio;
                    var delta = (destination - projectilePosition).normalized * parametersComponent.Speed * deltaTime;
                    transformComponent.Value.rotation = Quaternion.LookRotation(delta);

                    if (delta.magnitude >= direction.magnitude)
                    {
                        ref var ownerComponent = ref entity.GetComponent<OwnerComponent>(out var hasOwnerComponent);
                        ref var targetComponent = ref entity.GetComponent<TargetComponent>(out var hasTargetComponent);
                        
                        _projectileFlightEndEvent.NextFrame(new ProjectileFlightEndEvent
                        {
                            OwnerEntity = hasOwnerComponent ? ownerComponent.Entity : null,
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