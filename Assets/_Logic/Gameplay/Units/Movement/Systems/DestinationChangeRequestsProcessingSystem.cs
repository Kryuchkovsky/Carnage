using _Logic.Core;
using _Logic.Core.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    public sealed class DestinationChangeRequestsProcessingSystem : AbstractSystem
    {
        private Request<DestinationChangeRequest> _request;
        
        public override void OnAwake()
        {
            _request = World.GetRequest<DestinationChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (request.Entity.IsNullOrDisposed() || 
                    (request.Entity.TryGetComponentValue<TransformComponent>(out var transformComponent) && 
                     (request.Destination - transformComponent.Value.position).magnitude < 0.1f)) continue;
                
                if (request.Entity.Has<DestinationComponent>())
                {
                    ref var destinationComponent = ref request.Entity.GetComponent<DestinationComponent>();
                    
                    if (request.Destination == destinationComponent.Value) continue;

                    destinationComponent.Value = request.Destination;
                }
                else
                {
                    request.Entity.SetComponent(new DestinationComponent
                    {
                        Value = request.Destination
                    });
                }
                
                if (request.Entity.TryGetComponentValue<NavMeshAgentComponent>(out var navMeshAgentComponent))
                {
                    navMeshAgentComponent.Value.enabled = true;
                    navMeshAgentComponent.Value.SetDestination(request.Destination);
                }
            }
        }
    }
}