using _Logic.Core.Components;
using _Logic.Gameplay.Units.Projectiles.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Projectiles.Systems
{
    public sealed class ProjectileFlightProcessingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<ProjectileComponent>().With<ProjectileDataComponent>().With<TransformComponent>().With<DestinationComponent>()
                .ForEach((Entity entity, ref ProjectileComponent projectileComponent, ref ProjectileDataComponent projectileDataComponent, 
                    ref TransformComponent transformComponent, ref DestinationComponent destinationComponent) =>
                {
                    if (entity.TryGetComponentValue<FollowingTransformComponent>(out var followingTransformComponent))
                    {
                        destinationComponent.EndValue = followingTransformComponent.Value.position;
                    }

                    var projectilePosition = projectileComponent.Value.transform.position;
                    var direction = destinationComponent.EndValue - projectilePosition;
                    var distance = direction.magnitude;
                    var destination = destinationComponent.EndValue + Vector3.up * distance * projectileDataComponent.Value.FlightRangeToHeightRatio;
                    var delta = (destination - projectilePosition).normalized * projectileDataComponent.Value.Speed * deltaTime;
                    
                    transformComponent.Value.rotation = Quaternion.LookRotation(direction);

                    if (delta.magnitude >= distance)
                    {
                        entity.RemoveComponent<DestinationComponent>();
                        projectileComponent.Value.OnFlightEnded();
                    }
                    else
                    {
                        transformComponent.Value.position += delta;
                    }
                });
        }
    }
}