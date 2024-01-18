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
                        var transform = transformComponent.Value;
                        var direction = destinationData.Value - transform.position;

                        if (destinationData.Value == agent.destination || direction.magnitude <= agent.stoppingDistance)
                        {
                            //var rotation = Quaternion.LookRotation(direc tion);
                            // transform.rotation = Quaternion.RotateTowards(
                            //     transform.rotation, rotation, agent.angularSpeed * deltaTime);
                            //transform.rotation = rotation;
                            unitComponent.Value.OnMove(0);
                            agent.isStopped = true;

                        }
                        else
                        {
                            agent.SetDestination(destinationData.Value);
                            var speed = agent.velocity.magnitude / agent.speed;
                            unitComponent.Value.OnMove(speed);
                            agent.isStopped = false;
                        }
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