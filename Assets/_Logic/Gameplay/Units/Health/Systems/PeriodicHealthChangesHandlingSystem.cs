using _Logic.Core;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Requests;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Health.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PeriodicHealthChangesHandlingSystem : AbstractUpdateSystem
    {
        private FilterBuilder _healthFilter;
        private Request<HealthChangeRequest> _healthChangeRequest;
        private readonly float _interval = 0.5f;
        private float _time;
        
        public override void OnAwake()
        {
            _healthFilter = World.Filter.With<HealthComponent>().With<PeriodicHealthChangesComponent>().With<AliveComponent>();
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _healthFilter.Build())
            {
                ref var periodicHealthChangesComponent = ref entity.GetComponent<PeriodicHealthChangesComponent>();
                
                for (int i = 0; i < periodicHealthChangesComponent.Value.length;)
                {
                    ref var change = ref periodicHealthChangesComponent.Value.data[i];
                    change.LastChangeTime += deltaTime;
                    
                    if (change.LastChangeTime >= change.Interval)
                    {
                        change.LastChangeTime -= change.Interval;
                        var time = change.Interval == 0 ? 1 : change.Duration / change.Interval;

                        var changeData = new HealthChangeData
                        {
                            Type = change.Data.Type,
                            Value = change.Data.Value / time
                        };
                        _healthChangeRequest.Publish(new HealthChangeRequest
                        {
                            TargetEntity = entity,
                            Data = changeData,
                            CreatePopup = true
                        });
                    }

                    if (!change.IsPersist)
                    {
                        change.ExistingTime += deltaTime;
                    }

                    if ((!change.IsPersist && change.ExistingTime >= change.Duration) || change.Interval == 0)
                    {
                        periodicHealthChangesComponent.Value.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }
}