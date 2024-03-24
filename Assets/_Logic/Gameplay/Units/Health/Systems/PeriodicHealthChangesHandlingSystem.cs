using _Logic.Core;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
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
            _healthFilter = World.Filter.With<HealthComponent>();
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _healthFilter.Build())
            {
                ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                
                if (healthComponent.IsDead) continue;

                for (int i = 0; i < healthComponent.PeriodicHealthChanges.Count;)
                {
                    var change = healthComponent.PeriodicHealthChanges[i];

                    change.LastChangeTime += deltaTime;

                    if (change.LastChangeTime >= change.Interval)
                    {
                        change.LastChangeTime -= change.Interval;
                        
                        _healthChangeRequest.Publish(new HealthChangeRequest
                        {
                            TargetEntity = entity,
                            Data = new HealthChangeData
                            {
                                Type = change.Data.Type,
                                Value = change.Data.Value / (change.Duration / change.Interval)
                            }
                        });
                    }

                    if (!change.IsPersist)
                    {
                        change.ExistingTime += _interval;
                    }

                    if (change.ExistingTime >= change.Duration)
                    {
                        healthComponent.PeriodicHealthChanges.RemoveAt(i);
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