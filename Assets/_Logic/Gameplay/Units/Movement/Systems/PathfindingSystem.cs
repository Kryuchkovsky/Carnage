using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class PathfindingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitComponent> _unitStash;
        private Stash<StatsComponent> _statsStash;
        private Stash<DestinationComponent> _destinationStash;
        private Stash<PathfinderComponent> _pathfinderStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<MovementComponent>().With<StatsComponent>()
                .With<PathfinderComponent>().With<AIComponent>().With<AliveComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _statsStash = World.GetStash<StatsComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
            _pathfinderStash = World.GetStash<PathfinderComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var unitComponent = ref _unitStash.Get(entity);
                ref var statsComponent = ref _statsStash.Get(entity);
                ref var pathfinderComponent = ref _pathfinderStash.Get(entity);

                var path = pathfinderComponent.Value;
                var isCompleted = !path.hasPath || path.isStopped || path.reachedEndOfPath;

                if (isCompleted && _destinationStash.Has(entity))
                    _destinationStash.Remove(entity);

                var speed = statsComponent.Value.GetCurrentValue(StatType.MovementSpeed);
                pathfinderComponent.Value.maxSpeed = speed;
                
                var normalizedSpeed = path.velocity.magnitude / speed;
                unitComponent.Value.OnMove(normalizedSpeed);
            }
        }
    }
}