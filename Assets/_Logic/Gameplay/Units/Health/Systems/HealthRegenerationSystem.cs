using _Logic.Core;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Health.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthRegenerationSystem : AbstractUpdateSystem
    {
        private FilterBuilder _healthFilter;
        private readonly float _interval = 0.5f;
        private float _time;
        
        public override void OnAwake()
        {
            _healthFilter = World.Filter.With<HealthComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_time >= _interval)
            {
                foreach (var entity in _healthFilter.Build())
                {
                    ref var healthComponent = ref entity.GetComponent<HealthComponent>();
                    
                    if (healthComponent.IsDead) continue;

                    var health = healthComponent.Stats.MaxHealth.CurrentValue * healthComponent.Percentage; 
                    health += healthComponent.Stats.RegenerationRate.CurrentValue * _interval;
                    health = Mathf.Clamp(health, 0, healthComponent.Stats.MaxHealth.CurrentValue);
                    var percentage = health / healthComponent.Stats.MaxHealth.CurrentValue;

                    healthComponent.CurrentHealth = health;
                    healthComponent.Percentage = percentage;
                    
                    ref var healthBarComponent = ref entity.GetComponent<HealthBarComponent>(out var hasHealthBarComponent);
                    
                    if (hasHealthBarComponent)
                    {
                        healthBarComponent.Value.SetFillValue(healthComponent.Percentage);
                    }
                }
                    
                _time -= _interval;
            }
            else
            {
                _time += deltaTime;
            }
        }
    }
}