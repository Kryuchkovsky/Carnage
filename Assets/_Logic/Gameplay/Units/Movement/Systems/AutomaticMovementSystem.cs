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
                .With<UnitComponent>().With<TransformComponent>().With<MovementComponent>()
                .With<NavMeshAgentComponent>().With<DestinationComponent>().With<AIComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, 
                    ref TransformComponent transformComponent, ref MovementComponent movementComponent, 
                    ref NavMeshAgentComponent navMeshAgentComponent, ref DestinationComponent destinationData) =>
                {
                    var agent = navMeshAgentComponent.Value;
                    var position = transformComponent.Value.position;
                    var direction = destinationData.Value - position;
                    var isReached = destinationData.Value == position || 
                                    agent.remainingDistance <= agent.stoppingDistance || 
                                    direction.magnitude <= agent.stoppingDistance;
                    
                    if (isReached)
                    {
                        unitComponent.Value.OnMove(0);
                        entity.RemoveComponent<DestinationComponent>();
                    }
                    else
                    {
                        var speed = agent.velocity.magnitude / agent.speed;
                        unitComponent.Value.OnMove(speed);
                    }

                    agent.enabled = !isReached;
                    
                    if (entity.TryGetComponentValue<NavMeshObstacleComponent>(out var navMeshObstacleComponent))
                    {
                        navMeshObstacleComponent.Value.enabled = isReached;
                    }
                });
        }
    }
}