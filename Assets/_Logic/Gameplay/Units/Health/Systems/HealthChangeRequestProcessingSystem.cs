using System;
using _Logic.Core;
using _Logic.Extensions.Popup;
using _Logic.Extensions.VFXManager;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Events;
using _Logic.Gameplay.Units.Health.Requests;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Health.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthChangeRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<HealthChangeRequest> _healthChangeRequest;
        private Event<UnitDeathEvent> _unitDeathEvent;

        [Inject] private PopupsService _popupsService;
        [Inject] private VFXService _vfxService;
        
        public override void OnAwake()
        {
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _healthChangeRequest.Consume())
            {
                if (request.TargetEntity.IsNullOrDisposed() || !request.TargetEntity.Has<UnitComponent>() || 
                    !request.TargetEntity.Has<UnitDataComponent>() || !request.TargetEntity.Has<HealthComponent>() || 
                    !request.TargetEntity.Has<AliveComponent>() || !request.TargetEntity.Has<StatsComponent>()) continue;

                ref var unitComponent = ref request.TargetEntity.GetComponent<UnitComponent>();
                ref var unitDataComponent = ref request.TargetEntity.GetComponent<UnitDataComponent>();
                ref var healthComponent = ref request.TargetEntity.GetComponent<HealthComponent>();
                ref var statsComponent = ref request.TargetEntity.GetComponent<StatsComponent>();

                var maxHealth = statsComponent.Value.GetCurrentValue(StatType.MaxHeath);
                var health = maxHealth * healthComponent.Percentage;

                switch (request.Data.Type)
                {
                    case HealthChangeType.PhysicDamage:
                        health -= request.Data.Value;
                        _vfxService.CreateEffect(unitDataComponent.Value.DamageVFXType, unitComponent.Value.transform.position);
                        break;
                    case HealthChangeType.MagicDamage:
                        health -= request.Data.Value;
                        _vfxService.CreateEffect(unitDataComponent.Value.DamageVFXType, unitComponent.Value.transform.position);
                        break;
                    case HealthChangeType.Healing:
                        health += request.Data.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                health = Mathf.Clamp(health, 0, maxHealth);
                var percentage = health / maxHealth;

                healthComponent.CurrentHealth = health;
                healthComponent.Percentage = percentage;
                
                ref var healthBarComponent = ref request.TargetEntity.GetComponent<HealthBarComponent>(out var hasHealthBarComponent);
                    
                if (hasHealthBarComponent)
                {
                    healthBarComponent.Value.SetFillValue(healthComponent.Percentage);
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
                    
                    _popupsService.CreateWorldTextPopup(unitComponent.Value.transform, popupText, popupColor); 
                }
                    
                if (healthComponent.CurrentHealth <= 0)
                {
                    _unitDeathEvent.NextFrame(new UnitDeathEvent
                    {
                        CorpseEntity = request.TargetEntity,
                        MurdererEntity = request.SenderEntity
                    });
                    request.TargetEntity.RemoveComponent<AliveComponent>();
                    unitComponent.Value.OnDie();
                }
                else
                {
                    unitComponent.Value.OnDamage();
                }
            }
        }
    }
}