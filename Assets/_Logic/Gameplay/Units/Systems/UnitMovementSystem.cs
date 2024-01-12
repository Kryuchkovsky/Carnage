using _Logic.Core.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Systems
{
    public class UnitMovementSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<NavMeshAgentComponent>().With<DestinationComponent>()
                .ForEach((Entity e, ref UnitComponent unitLink, ref NavMeshAgentComponent navMeshAgentLink, ref DestinationComponent destinationData) =>
                {
                    navMeshAgentLink.Value.SetDestination(destinationData.Value);
                    var speed = navMeshAgentLink.Value.velocity.magnitude / navMeshAgentLink.Value.speed;
                    unitLink.Value.SetMovementSpeed(speed);
                });
        }
    }
}