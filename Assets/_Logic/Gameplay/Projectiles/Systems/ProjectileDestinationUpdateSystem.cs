using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Projectiles.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Projectiles.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileDestinationUpdateSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<TargetComponent> _targetStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<BoundsComponent> _boundsStash;
        private Stash<DestinationComponent> _destinationStash;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<TargetComponent>().With<FlightParametersComponent>().Build();
            _targetStash = World.GetStash<TargetComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _boundsStash = World.GetStash<BoundsComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var targetComponent = ref _targetStash.Get(entity);
                ref var targetTransformComponent = ref _transformStash.Get(targetComponent.Entity, out var targetHasTransformComponent);
                        
                if (targetHasTransformComponent)
                {
                    var targetPosition = targetTransformComponent.Value.position;
                    ref var targetBoundsComponent = ref _boundsStash.Get(targetComponent.Entity, out var targetHasBoundsComponent);

                    if (targetHasBoundsComponent)
                    {
                        targetPosition += Vector3.up * targetBoundsComponent.Value.extents.y;
                    }
                        
                    _destinationStash.Set(entity, new DestinationComponent
                    {
                        Value = targetPosition
                    });
                }
            }
        }
    }
}