using System;
using System.Collections.Generic;
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
        private Dictionary<HealthChangeType, PopupData> _popupDataSet;
        
        private Request<HealthChangeRequest> _healthChangeRequest;
        private Event<UnitDeathEvent> _unitDeathEvent;

        [Inject] private PopupsService _popupsService;
        [Inject] private VFXService _vfxService;
        
        public override void OnAwake()
        {
            _popupDataSet = new Dictionary<HealthChangeType, PopupData>();

            foreach (var type in (HealthChangeType[])Enum.GetValues(typeof(HealthChangeType)))
            {
                var data = GetPopupData(type);
                _popupDataSet.Add(type, data);
            }
            
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
                    var popupData = _popupDataSet[request.Data.Type];
                    _popupsService.CreateTextPopup(popupData.Id, unitComponent.Value.transform.position, request.Data.Value, popupData.Color); 
                }
                    
                if (healthComponent.CurrentHealth <= 0.1f)
                {
                    _vfxService.CreateEffect(unitDataComponent.Value.DeathVFXType, unitComponent.Value.transform.position);
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

        private PopupData GetPopupData(HealthChangeType type)
        {
            string popupFormat;
            Color popupColor;

            switch (type)
            {
                case HealthChangeType.PhysicDamage:
                    popupFormat = "-{0:1}";
                    popupColor = Color.red;
                    break;
                case HealthChangeType.MagicDamage:
                    popupFormat = "-{0:1}";
                    popupColor = Color.blue;
                    break;
                case HealthChangeType.Healing:
                    popupFormat = "+{0:1}";
                    popupColor = Color.green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var id = _popupsService.RegisterPopupAndGetId(popupFormat);
            return new PopupData(popupColor, id);
        }
        
        private class PopupData
        {
            public readonly Color Color;
            public readonly int Id;

            public PopupData(Color color, int id)
            {
                Color = color;
                Id = id;
            }
        }
    }
}