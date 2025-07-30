using _Logic.Core;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
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
        private Filter _healthFilter;
        private Stash<HealthComponent> _healthStash;
        private Stash<HealthBarComponent> _healthBarStash;
        private Stash<StatsComponent> _statsStash;
        private readonly float _interval = 0.5f;
        private float _time;
        
        public override void OnAwake()
        {
            _healthFilter = World.Filter.With<HealthComponent>().With<StatsComponent>().With<AliveComponent>().Build();
            _healthStash = World.GetStash<HealthComponent>();
            _healthBarStash = World.GetStash<HealthBarComponent>();
            _statsStash = World.GetStash<StatsComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_time >= _interval)
            {
                foreach (var entity in _healthFilter)
                {
                    ref var healthComponent = ref _healthStash.Get(entity);
                    ref var statsComponent = ref _statsStash.Get(entity);

                    var maxHealth = statsComponent.Value.GetCurrentValue(StatType.MaxHeath);
                    var regenerationRate = statsComponent.Value.GetCurrentValue(StatType.HealthRegenerationRate);
                    
                    var health = maxHealth * healthComponent.Percentage; 
                    health += regenerationRate * _interval;
                    health = Mathf.Clamp(health, 0, maxHealth);
                    var percentage = health / maxHealth;

                    healthComponent.CurrentHealth = health;
                    healthComponent.Percentage = percentage;
                    
                    ref var healthBarComponent = ref _healthBarStash.Get(entity, out var hasHealthBarComponent);
                    
                    if (hasHealthBarComponent)
                        healthBarComponent.Value.SetFillValue(healthComponent.Percentage);
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