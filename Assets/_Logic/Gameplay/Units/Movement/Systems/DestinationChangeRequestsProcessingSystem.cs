using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestinationChangeRequestsProcessingSystem : AbstractUpdateSystem
    {
        private Request<DestinationChangeRequest> _request;
        private Stash<PathfinderComponent> _pathfinderStash;
        private Stash<DestinationComponent> _destinationStash;
        private Stash<AliveComponent> _aliveStash;
        private Stash<TransformComponent> _transformStash;

        public override void OnAwake()
        {
            _request = World.GetRequest<DestinationChangeRequest>();
            _pathfinderStash = World.GetStash<PathfinderComponent>();
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

                ref var pathfinderComponent = ref _pathfinderStash.Get(request.Entity, out var hasPathfinderComponent);
                
                if (hasPathfinderComponent)
                {
                    //todo refactor
                }
            }
        }
    }
}