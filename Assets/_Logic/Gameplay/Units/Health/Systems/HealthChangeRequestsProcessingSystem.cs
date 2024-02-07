using _Logic.Core;
using _Logic.Extensions.VFXManager;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health.Systems
{
    public sealed class HealthChangeRequestsProcessingSystem : AbstractSystem
    {
        private Request<HealthChangeRequest> _request;
        
        public override void OnAwake()
        {
            _request = World.GetRequest<HealthChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (request.Entity.IsNullOrDisposed() || !request.Entity.Has<HealthComponent>()) return;

                ref var healthComponent = ref request.Entity.GetComponent<HealthComponent>();
                healthComponent.Value += request.Change;
                healthComponent.Percentage = healthComponent.Value / healthComponent.CurrentData.MaxValue;
                    
                if (request.Entity.TryGetComponentValue<UnitComponent>(out var unitComponent))
                {
                    var unitProvider = unitComponent.Value;
                    
                    if (healthComponent.Value <= 0)
                    {
                        unitProvider.OnDie();
                        request.Entity.Dispose();
                    }
                    else
                    {
                        unitProvider.OnDamage();
                    }
                    
                }
            }
        }
    }
}