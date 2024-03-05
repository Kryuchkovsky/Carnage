using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;


namespace _Logic.Gameplay.Units.Movement.Systems
{
    public class AutomaticMovementSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<MovementComponent>().With<NavMeshAgentComponent>().With<AIComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref MovementComponent movementComponent, ref NavMeshAgentComponent navMeshAgentComponent) =>
                {
                    var agent = navMeshAgentComponent.Value;
                    var isCompleted = !agent.hasPath || agent.isStopped;

                    if (isCompleted && entity.Has<DestinationComponent>())
                    {
                        entity.RemoveComponent<DestinationComponent>();
                    }

                    agent.speed = movementComponent.Stats.MovementSpeed.CurrentValue;
                    var speed = agent.velocity.magnitude / agent.speed;
                    unitComponent.Value.OnMove(speed);
                    //agent.enabled = !isCompleted;

                    ref var obstacleComponent = ref entity.GetComponent<NavMeshObstacleComponent>(out var hasObstacleComponent);
                    
                    if (hasObstacleComponent)
                    {
                        //obstacleComponent.Value.enabled = isCompleted;
                    }
                });
        }
    }
}