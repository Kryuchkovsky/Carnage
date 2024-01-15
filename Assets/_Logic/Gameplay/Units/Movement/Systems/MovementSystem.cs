using _Logic.Core.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    public class MovementSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<TransformComponent>().With<MovementComponent>().With<DestinationComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref TransformComponent transformComponent, 
                    ref MovementComponent movementComponent, ref DestinationComponent destinationData) =>
                {
                    if (entity.Has<NavMeshAgentComponent>())
                    {
                        var agent = entity.GetComponent<NavMeshAgentComponent>().Value;
                        agent.SetDestination(destinationData.Value);
                        var speed = agent.velocity.magnitude / agent.speed;
                        unitComponent.Value.SetMovementSpeed(speed);
                    }
                    else
                    {
                        var step = (destinationData.Value - transformComponent.Value.position).normalized * 
                                        movementComponent.CurrentData.MovementSpeed * deltaTime;
                        transformComponent.Value.Translate(step);
                    }
                });
        }
    }
}