using _Logic.Core;
using _Logic.Extensions.HealthBar;
using _Logic.Extensions.Popup;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Events;
using _Logic.Gameplay.Units.Stats;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Health.Systems
{
    public sealed class HealthChangeRequestsProcessingSystem : AbstractUpdateSystem
    {
        private Request<HealthChangeRequest> _healthChangeRequest;
        private Event<UnitDeathEvent> _unitDeathEvent;
        
        public override void OnAwake()
        {
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _healthChangeRequest.Consume())
            {
                if (request.TargetEntity.IsNullOrDisposed() || !request.TargetEntity.Has<UnitComponent>() || !request.TargetEntity.Has<HealthComponent>()) continue;

                ref var unitComponent = ref request.TargetEntity.GetComponent<UnitComponent>();
                ref var healthComponent = ref request.TargetEntity.GetComponent<HealthComponent>();
                
                if (healthComponent.IsDead) continue;
                
                healthComponent.Stats.CurrentHealth.AddModifier(new StatModifier(StatModifierOperationType.Addition, request.Change));
                healthComponent.Percentage = healthComponent.Stats.CurrentHealth.CurrentValue / healthComponent.Stats.MaxHealth.CurrentValue;
                
                ref var healthBarComponent = ref request.TargetEntity.GetComponent<HealthBarComponent>(out var hasHealthBarComponent);
                    
                if (hasHealthBarComponent)
                {
                    healthBarComponent.Value.SetFillValue(healthComponent.Percentage);
                        
                    if ( healthComponent.Stats.CurrentHealth.CurrentValue <= 0)
                    {
                        HealthBarsService.Instance.RemoveHealthBar(healthBarComponent.Value);
                    }
                }

                var popupColor = request.Change >= 0 ? Color.green : Color.red;
                var popupText = request.Change >= 0 ? $"+{request.Change}" : $"{request.Change}";
                PopupsService.Instance.CreateWorldTextPopup(unitComponent.Value.transform, popupText, popupColor);
                    
                if (healthComponent.Stats.CurrentHealth.CurrentValue <= 0)
                {
                    _unitDeathEvent.NextFrame(new UnitDeathEvent
                    {
                        CorpseEntity = request.TargetEntity,
                        MurdererEntity = request.SenderEntity
                    });
                    healthComponent.IsDead = true;
                }
                else
                {
                    unitComponent.Value.OnDamage();
                }
            }
        }
    }
}