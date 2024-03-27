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
    public class AutomaticMovementSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<MovementComponent>().With<StatsComponent>().With<NavMeshAgentComponent>().With<AIComponent>().With<AliveComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref MovementComponent movementComponent, ref StatsComponent statsComponent, ref NavMeshAgentComponent navMeshAgentComponent) =>
                {
                    var agent = navMeshAgentComponent.Value;
                    var isCompleted = !agent.hasPath || agent.isStopped;

                    if (isCompleted && entity.Has<DestinationComponent>())
                    {
                        entity.RemoveComponent<DestinationComponent>();
                    }

                    if (statsComponent.Value.TryGetCurrentValue(StatType.MovementSpeed, out var speed))
                    {
                        agent.speed = speed;
                        var normalizedSpeed = agent.velocity.magnitude / speed;
                        unitComponent.Value.OnMove(normalizedSpeed);
                        //agent.enabled = !isCompleted;

                        ref var obstacleComponent = ref entity.GetComponent<NavMeshObstacleComponent>(out var hasObstacleComponent);
                    
                        if (hasObstacleComponent)
                        {
                            //obstacleComponent.Value.enabled = isCompleted;
                        }
                    }
                });
        }
    }
}