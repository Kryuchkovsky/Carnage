using _Logic.Core;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health.Systems
{
    public class HealthChangeRequestsProcessingSystem : AbstractSystem
    {
        private Request<HealthChangeRequest> _request;
        private UnitCatalog _unitCatalog;

        public override void OnAwake()
        {
            _request = World.GetRequest<HealthChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (request.Entity.Has<HealthComponent>())
                {
                    ref var healthComponent = ref request.Entity.GetComponent<HealthComponent>();
                    healthComponent.Value += request.Change;
                    
                    if (healthComponent.Value <= 0)
                    {
                        if (request.Entity.Has<UnitComponent>())
                        {
                            request.Entity.GetComponent<UnitComponent>().Value.Kill();
                        }
                        
                        request.Entity.Dispose();
                    }
                }
            }
        }
    }
}