using _Logic.Core;
using _Logic.Core.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    public sealed class DestinationChangeRequestsProcessingSystem : AbstractUpdateSystem
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
                if (request.Entity.IsNullOrDisposed() || !request.Entity.Has<TransformComponent>()) 
                    continue;
                
                var transformComponent = request.Entity.GetComponent<TransformComponent>();
                var position = transformComponent.Value.position;
                var direction = request.Destination - position;
                
                if (direction.magnitude < 0.1f) 
                    continue;
                
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

                var agentComponent = request.Entity.GetComponent<NavMeshAgentComponent>(out var hasAgentComponent);
                
                if (hasAgentComponent)
                {
                    var destination = request.Destination - direction.normalized * 0.1f;
                    agentComponent.Value.enabled = true;
                    agentComponent.Value.SetDestination(destination);
                }
            }
        }
    }
}