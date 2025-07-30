using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    public class AutomaticMovementSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitComponent> _unitStash;
        private Stash<StatsComponent> _statsStash;
        private Stash<DestinationComponent> _destinationStash;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        private Stash<NavMeshObstacleComponent> _navMeshObstacleStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<MovementComponent>().With<StatsComponent>()
                .With<NavMeshAgentComponent>().With<AIComponent>().With<AliveComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _statsStash = World.GetStash<StatsComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            _navMeshObstacleStash = World.GetStash<NavMeshObstacleComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var unitComponent = ref _unitStash.Get(entity);
                ref var statsComponent = ref _statsStash.Get(entity);
                ref var navMeshAgentComponent = ref _navMeshAgentStash.Get(entity);

                var agent = navMeshAgentComponent.Value;
                var isCompleted = !agent.hasPath || agent.isStopped;

                if (isCompleted && _destinationStash.Has(entity))
                    _destinationStash.Remove(entity);

                var speed = statsComponent.Value.GetCurrentValue(StatType.MovementSpeed);
                agent.speed = speed;
                var normalizedSpeed = agent.velocity.magnitude / speed;
                unitComponent.Value.OnMove(normalizedSpeed);
                //agent.enabled = !isCompleted;

                ref var obstacleComponent = ref _navMeshObstacleStash.Get(entity, out var hasObstacleComponent);
                    
                if (hasObstacleComponent)
                {
                    //obstacleComponent.Value.enabled = isCompleted;
                }
            }
        }
    }
}