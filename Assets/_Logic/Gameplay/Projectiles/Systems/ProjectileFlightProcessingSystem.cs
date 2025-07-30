using System;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Projectiles.Components;
using _Logic.Gameplay.Projectiles.Events;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Projectiles.Systems
{
    public sealed class ProjectileFlightProcessingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<FlightParametersComponent> _flightParametersStash;
        private Stash<DestinationComponent> _destinationStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<OwnerComponent> _ownerStash;
        private Stash<TargetComponent> _targetStash;
        private Event<ProjectileFlightEndEvent> _projectileFlightEndEvent;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<FlightParametersComponent>().With<DestinationComponent>().With<TransformComponent>().Build();
            _flightParametersStash = World.GetStash<FlightParametersComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _ownerStash = World.GetStash<OwnerComponent>();
            _targetStash = World.GetStash<TargetComponent>();
            _projectileFlightEndEvent = World.GetEvent<ProjectileFlightEndEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var parametersComponent = ref _flightParametersStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var destinationComponent = ref _destinationStash.Get(entity);
                
                var projectilePosition = transformComponent.Value.position;
                var direction = destinationComponent.Value - projectilePosition;
                var distanceWithoutHeight = new Vector3(direction.x, 0, direction.z).magnitude;
                var destination = destinationComponent.Value + Vector3.up * distanceWithoutHeight * parametersComponent.FlightRangeToHeightRatio;
                var delta = (destination - projectilePosition).normalized * parametersComponent.Speed * deltaTime;
                transformComponent.Value.rotation = Quaternion.LookRotation(delta);

                if (delta.magnitude >= direction.magnitude)
                {
                    ref var ownerComponent = ref _ownerStash.Get(entity, out var hasOwnerComponent);
                    ref var targetComponent = ref _targetStash.Get(entity, out var hasTargetComponent);
                        
                    _projectileFlightEndEvent.NextFrame(new ProjectileFlightEndEvent
                    {
                        OwnerEntity = hasOwnerComponent ? ownerComponent.Entity : new Entity(),
                        TargetEntity = hasTargetComponent ? targetComponent.Entity : new Entity(),
                        ProjectileEntity = entity
                    });

                    if (hasTargetComponent)
                    {
                        _targetStash.Remove(entity);
                    }

                    _destinationStash.Remove(entity);
                }
                else
                {
                    transformComponent.Value.position += delta;
                }
            }
        }
    }
}