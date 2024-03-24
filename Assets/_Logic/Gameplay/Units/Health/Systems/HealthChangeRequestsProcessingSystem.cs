using System;
using _Logic.Core;
using _Logic.Extensions.HealthBar;
using _Logic.Extensions.Popup;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Health.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
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
                
                var health = healthComponent.Stats.MaxHealth.CurrentValue * healthComponent.Percentage;

                switch (request.Data.Type)
                {
                    case HealthChangeType.PhysicDamage:
                        health -= request.Data.Value;
                        break;
                    case HealthChangeType.MagicDamage:
                        health -= request.Data.Value;
                        break;
                    case HealthChangeType.Healing:
                        health += request.Data.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                health = Mathf.Clamp(health, 0, healthComponent.Stats.MaxHealth.CurrentValue);
                var percentage = health / healthComponent.Stats.MaxHealth.CurrentValue;

                healthComponent.CurrentHealth = health;
                healthComponent.Percentage = percentage;
                
                ref var healthBarComponent = ref request.TargetEntity.GetComponent<HealthBarComponent>(out var hasHealthBarComponent);
                    
                if (hasHealthBarComponent)
                {
                    healthBarComponent.Value.SetFillValue(healthComponent.Percentage);
                        
                    if (healthComponent.CurrentHealth <= 0)
                    {
                        HealthBarsService.Instance.RemoveHealthBar(healthBarComponent.Value);
                    }
                }

                if (request.CreatePopup)
                {
                    Color popupColor;
                    string popupText;
                    
                    switch (request.Data.Type)
                    {
                        case HealthChangeType.PhysicDamage:
                            popupColor = Color.red;
                            popupText = $"-{request.Data.Value}";
                            break;
                        case HealthChangeType.MagicDamage:
                            popupColor = Color.blue;
                            popupText = $"-{request.Data.Value}";
                            break;
                        case HealthChangeType.Healing:
                            popupColor = Color.green;
                            popupText = $"+{request.Data.Value}";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    PopupsService.Instance.CreateWorldTextPopup(unitComponent.Value.transform, popupText, popupColor); 
                }
                    
                if (healthComponent.CurrentHealth <= 0)
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