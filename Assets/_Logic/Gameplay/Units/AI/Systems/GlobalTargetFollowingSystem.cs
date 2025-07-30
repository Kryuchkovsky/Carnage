using System.Collections.Generic;
using System.Linq;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class GlobalTargetFollowingSystem : AbstractUpdateSystem
    {
        private Dictionary<int, PriorityTargetData> _dictionary;
        private Filter _prioritizedTargetsFilter;
        private Filter _unitsFilter;
        private bool _prioritizedTargetsAreUpdated;
        
        public override void OnAwake()
        {
            _dictionary = new Dictionary<int, PriorityTargetData>();
            _prioritizedTargetsFilter = World.Filter.With<UnitComponent>().With<HealthComponent>().With<AliveComponent>().With<PriorityComponent>().With<TeamComponent>().Build();
            _unitsFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<AttackComponent>().With<MovementComponent>().With<AIComponent>()
                .Without<AttackTargetComponent>().Without<DestinationComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            _prioritizedTargetsAreUpdated = false;
            
            foreach (var entity in _unitsFilter)
            {
                if (!_prioritizedTargetsAreUpdated)
                {
                    UpdatePositionsOfPrioritizedTargets();
                }
                
                var teamId = entity.GetComponent<TeamComponent>().Id;
                var pair = _dictionary.Where(p => p.Key != teamId)
                    .Select(p => (KeyValuePair<int, PriorityTargetData>?) p)
                    .FirstOrDefault();

                if (pair != null)
                {
                    var enemyTeamId = pair.Value.Key;
                    var targetEntity = _dictionary[enemyTeamId].Entity;
                    
                    entity.SetComponent(new AttackTargetComponent
                    {
                        TargetEntity = targetEntity
                    });
                }
            }
        }

        private void UpdatePositionsOfPrioritizedTargets()
        {
            foreach (var entity in _prioritizedTargetsFilter)
            {
                var teamId = entity.GetComponent<TeamComponent>().Id;
                var transform = entity.GetComponent<TransformComponent>().Value;
                var priority = entity.GetComponent<PriorityComponent>().Value;

                _dictionary.TryAdd(teamId, new PriorityTargetData
                {
                    Entity = entity,
                    Transform = transform,
                    Priority = priority
                });

                if (_dictionary[teamId].Entity.IsNullOrDisposed() || priority < _dictionary[teamId].Priority)
                {
                    _dictionary[teamId].Entity = entity;
                    _dictionary[teamId].Transform = transform;
                    _dictionary[teamId].Priority = priority;
                }
            }

            _prioritizedTargetsAreUpdated = true;
        }
        
        private class PriorityTargetData
        {
            public Entity Entity;
            public Transform Transform;
            public int Priority;
        }
    }
}