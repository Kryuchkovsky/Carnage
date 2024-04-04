using System.Collections.Generic;
using _Logic.Core;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Requests;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Health.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthChangeProcessAdditionRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<HealthChangeProcessAdditionRequest> _healthChangeProcessAdditionRequest;
        
        public override void OnAwake()
        {
            _healthChangeProcessAdditionRequest = World.GetRequest<HealthChangeProcessAdditionRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _healthChangeProcessAdditionRequest.Consume())
            {
                if (request.TargetEntity.IsNullOrDisposed() || !request.TargetEntity.Has<UnitComponent>() || 
                    !request.TargetEntity.Has<HealthComponent>() || !request.TargetEntity.Has<AliveComponent>() || 
                    !request.TargetEntity.Has<StatsComponent>()) continue;
                
                ref var periodicHealthChangesComponent = ref request.TargetEntity.GetComponent<PeriodicHealthChangesComponent>(out var hasPeriodicHealthChangesComponent);

                if (hasPeriodicHealthChangesComponent)
                {
                    periodicHealthChangesComponent.Value.Add(request.Process);
                }
                else
                {
                    var processes = new FastList<HealthChangeProcess>();
                    processes.Add(request.Process);
                    
                    request.TargetEntity.SetComponent(new PeriodicHealthChangesComponent
                    {
                        Value = processes
                    });
                }
            }
        }
    }
}