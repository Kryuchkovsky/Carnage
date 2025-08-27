using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    public sealed class DestinationChangeRequestsProcessingSystem : AbstractUpdateSystem
    {
        private Request<DestinationChangeRequest> _request;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        private Stash<DestinationComponent> _destinationStash;
        private Stash<AliveComponent> _aliveStash;
        private Stash<TransformComponent> _transformStash;

        public override void OnAwake()
        {
            _request = World.GetRequest<DestinationChangeRequest>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
            _aliveStash = World.GetStash<AliveComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (World.IsDisposed(request.Entity) || !_transformStash.Has(request.Entity) || !_aliveStash.Has(request.Entity)) 
                    continue;

                var transformComponent = _transformStash.Get(request.Entity);
                var position = transformComponent.Value.position;
                var direction = request.Destination - position;
                
                if (direction.magnitude < 0.1f) 
                    continue;

                ref var destinationComponent = ref _destinationStash.Get(request.Entity, out var hasDestinationComponent);
                
                if (hasDestinationComponent)
                {
                    if (request.Destination == destinationComponent.Value) 
                        continue;

                    destinationComponent.Value = request.Destination;
                }
                else
                {
                    _destinationStash.Set(request.Entity, new DestinationComponent
                    {
                        Value = request.Destination
                    });
                }

                var agentComponent = _navMeshAgentStash.Get(request.Entity, out var hasAgentComponent);
                
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