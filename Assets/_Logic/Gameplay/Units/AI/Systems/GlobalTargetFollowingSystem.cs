using System.Collections.Generic;
using System.Linq;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Components;
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
    public sealed class GlobalTargetFollowingSystem : AbstractSystem
    {
        private Dictionary<int, PriorityTargetData> _dictionary;
        private FilterBuilder _prioritizedTargetsFilter;
        private FilterBuilder _unitsFilter;
        private bool _prioritizedTargetsAreUpdated;
        
        public override void OnAwake()
        {
            _dictionary = new Dictionary<int, PriorityTargetData>();
            _prioritizedTargetsFilter = World.Filter.With<UnitComponent>().With<HealthComponent>().With<PriorityComponent>().With<TeamIdComponent>();
            _unitsFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<AttackComponent>().With<MovementComponent>().With<AIComponent>()
                .Without<AttackTargetComponent>().Without<DestinationComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            _prioritizedTargetsAreUpdated = false;
            
            foreach (var entity in _unitsFilter.Build())
            {
                if (!_prioritizedTargetsAreUpdated)
                {
                    UpdatePositionsOfPrioritizedTargets();
                }

                var position = entity.GetComponent<TransformComponent>().Value.position;
                var teamId = entity.GetComponent<TeamIdComponent>().Value;
                var enemyTeamId = _dictionary.First(i => i.Key != teamId).Key;
                var destination = _dictionary[enemyTeamId].Position + (position - _dictionary[enemyTeamId].Position).normalized;

                World.GetRequest<DestinationChangeRequest>().Publish(new DestinationChangeRequest
                {
                    Entity = entity,
                    Destination = destination
                });
            }
        }

        private void UpdatePositionsOfPrioritizedTargets()
        {
            foreach (var entity in _prioritizedTargetsFilter.Build())
            {
                var teamId = entity.GetComponent<TeamIdComponent>().Value;
                var priority = entity.GetComponent<PriorityComponent>().Value;
                var position = entity.GetComponent<TransformComponent>().Value.position;

                _dictionary.TryAdd(teamId, new PriorityTargetData
                {
                    Entity = entity,
                    Position = position,
                    Priority = priority
                });

                if (_dictionary[teamId].Entity.IsNullOrDisposed() || _dictionary[teamId].Priority < priority)
                {
                    _dictionary[teamId].Entity = entity;
                    _dictionary[teamId].Position = position;
                    _dictionary[teamId].Priority = priority;
                }
            }

            _prioritizedTargetsAreUpdated = true;
        }
        
        private class PriorityTargetData
        {
            public Entity Entity;
            public Vector3 Position;
            public int Priority;
        }
    }
}