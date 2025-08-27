using _Logic.Core;
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
    public sealed class AttackTargetFollowingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        private Stash<AttackTargetComponent> _attackTargetStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<DestinationComponent> _destinationStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<MovementComponent>().With<NavMeshAgentComponent>()
                .With<AttackComponent>().With<AliveComponent>().With<AIComponent>().Build();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            _attackTargetStash = World.GetStash<AttackTargetComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var targetComponent = ref _attackTargetStash.Get(entity, out var hasTargetComponent);
                ref var navMeshAgentComponent = ref _navMeshAgentStash.Get(entity);
                var hasTarget = hasTargetComponent && !World.IsDisposed(targetComponent.TargetEntity);
                var isStopped = true;

                if (hasTarget)
                {
                    ref var targetTransformComponent = ref _transformStash.Get(targetComponent.TargetEntity, out var targetHasTransform);
                    isStopped = targetHasTransform && targetComponent.IsInAttackRadius;
                    
                    ref var destinationComponent = ref _destinationStash.Get(entity, out var hasDestinationComponent);

                    if (targetHasTransform && (!hasDestinationComponent || destinationComponent.Value != targetTransformComponent.Value.position))
                    {
                        World.GetRequest<DestinationChangeRequest>().Publish(new DestinationChangeRequest
                        {
                            Entity = entity,
                            Destination = targetTransformComponent.Value.position
                        });
                    }
                }
                
                if (navMeshAgentComponent.Value.enabled)
                {
                    navMeshAgentComponent.Value.isStopped = isStopped;
                        
                    if (isStopped)
                        navMeshAgentComponent.Value.velocity = Vector3.zero;
                }
            }
        }
    }
}