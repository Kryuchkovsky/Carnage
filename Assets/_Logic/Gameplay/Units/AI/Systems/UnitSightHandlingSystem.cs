using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class UnitSightHandlingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitComponent> _unitStash;
        private Stash<StatsComponent> _statsStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<AttackTargetComponent> _attackTargetStash;
        private Stash<PathfinderComponent> _pathfinderStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<StatsComponent>().With<AttackComponent>().With<TransformComponent>()
                .With<AliveComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _statsStash = World.GetStash<StatsComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _attackTargetStash = World.GetStash<AttackTargetComponent>();
            _pathfinderStash = World.GetStash<PathfinderComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var unitComponent = ref _unitStash.Get(entity);
                ref var statsComponent = ref _statsStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var targetComponent = ref _attackTargetStash.Get(entity, out var hasTargetComponent);
                ref var pathfinderComponent = ref _pathfinderStash.Get(entity, out var hasPathfinderComponent);
                var sightPosition = Vector3.zero;

                if (hasTargetComponent && !World.IsDisposed(targetComponent.TargetEntity) &&
                    _transformStash.Has(targetComponent.TargetEntity) && targetComponent.IsInAttackRadius)
                {
                    sightPosition = _transformStash.Get(targetComponent.TargetEntity).Value.position;
                }
                else if (!hasPathfinderComponent || pathfinderComponent.Value.velocity != Vector3.zero)
                {
                    sightPosition = transformComponent.Value.position + transformComponent.Value.forward;
                }

                if (sightPosition != Vector3.zero)
                {
                    var rotationStep = statsComponent.Value.GetCurrentValue(StatType.RotationSpeed) * deltaTime;
                    unitComponent.Value.Model.LookAtPoint(sightPosition, rotationStep);  
                }
            }
        }
    }
}