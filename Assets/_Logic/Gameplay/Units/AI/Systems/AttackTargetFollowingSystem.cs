﻿using _Logic.Core;
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
        private Stash<DestinationComponent> _transformStash;
        private Stash<TransformComponent> _destinationStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<MovementComponent>().With<NavMeshAgentComponent>()
                .With<AttackComponent>().With<AttackTargetComponent>().With<AliveComponent>().With<AIComponent>().Build();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            _attackTargetStash = World.GetStash<AttackTargetComponent>();
            _transformStash = World.GetStash<DestinationComponent>();
            _destinationStash = World.GetStash<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var targetComponent = ref _attackTargetStash.Get(entity);
                ref var navMeshAgentComponent = ref _navMeshAgentStash.Get(entity);
                
                if (World.IsDisposed(targetComponent.TargetEntity)) 
                    return;
                    
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
            }
        }
    }
}