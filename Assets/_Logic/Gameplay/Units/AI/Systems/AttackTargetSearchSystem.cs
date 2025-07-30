using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class AttackTargetSearchSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<StatsComponent> _statsStash;
        private Stash<AttackTargetComponent> _attackTargetStash;
        private Stash<TeamComponent> _teamStash;
        private Stash<TransformComponent> _transformStash;
        private readonly Collider[] _colliders = new Collider[10];

        [Inject] private AISettings _aiSettings;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<StatsComponent>().With<AttackComponent>()
                .With<TeamComponent>().With<TransformComponent>().With<AliveComponent>()
                .Without<AttackTargetComponent>().Build();
            _statsStash = World.GetStash<StatsComponent>();
            _attackTargetStash = World.GetStash<AttackTargetComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var statsComponent = ref _statsStash.Get(entity);
                ref var teamComponent = ref _teamStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                
                var position = transformComponent.Value.position;
                var range = statsComponent.Value.GetCurrentValue(StatType.AttackRange);
                var searchRange  = range * _aiSettings.TargetSearchRangeToAttackRangeRatio;
                var collisions = Physics.OverlapSphereNonAlloc(position, searchRange, _colliders, 1 << teamComponent.EnemiesLayer);
                var minDistance = float.MaxValue;
                
                LinkedCollider targetLinkedCollider = null;
                    
                for (int i = 0; i < collisions; i++)
                {
                    if (_colliders[i].TryGetComponent(out LinkedCollider collider) &&
                        !World.IsDisposed(collider.Entity) && 
                        _teamStash.Get(collider.Entity).Id != teamComponent.Id)
                    {
                        if (EcsExtensions.TryGetDistanceBetweenClosestPoints(entity, collider.Entity, out var distance) && 
                            distance < searchRange && distance < minDistance)
                        {
                            minDistance = distance;
                            targetLinkedCollider = collider;
                        }
                    }
                }

                if (targetLinkedCollider is not null)
                {
                    _attackTargetStash.Set(entity, new AttackTargetComponent
                    {
                        TargetEntity = targetLinkedCollider.Entity
                    });
                }
            }
        }
    }
}