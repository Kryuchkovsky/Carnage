using _Logic.Core;
using _Logic.Extensions.HealthBar;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Spawn;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Health.Systems
{
    public sealed class HealthBarAddingSystem : AbstractSystem
    {
        private Event<UnitSpawnEvent> _event;
        
        public override void OnAwake()
        {
            _event = World.GetEvent<UnitSpawnEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var unitSpawnEvent in _event.publishedChanges)
            {
                var entity = unitSpawnEvent.UnitProvider.Entity;
                
                if (entity.IsNullOrDisposed() || !entity.Has<HealthComponent>() || entity.Has<HealthBarComponent>()) return;
                
                var isAlly = entity.TryGetComponentValue<TeamIdComponent>(out var teamIdComponent) && teamIdComponent.Value == 0;
                var healthBar = HealthBarCreationService.Instance.CreateHealthBar(unitSpawnEvent.UnitProvider.transform, 3, isAlly);
                entity.SetComponent(new HealthBarComponent
                {
                    Value = healthBar
                });
            }
        }
    }
}