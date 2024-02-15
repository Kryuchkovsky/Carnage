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
                    var agent = entity.GetComponent<NavMeshAgentComponent>().Value;
                    var transform = transformComponent.Value;
                    var direction = destinationData.Value - transform.position;
                    
                    if (destinationData.Value == transform.position || direction.magnitude <= agent.stoppingDistance)
                    {
                        unitComponent.Value.OnMove(0);
                        agent.isStopped = true;
                        entity.RemoveComponent<DestinationComponent>();
                    }
                    else
                    {
                        var speed = agent.velocity.magnitude / agent.speed;
                        unitComponent.Value.OnMove(speed);
                        agent.isStopped = false;
                    }
                });
        }
    }
}