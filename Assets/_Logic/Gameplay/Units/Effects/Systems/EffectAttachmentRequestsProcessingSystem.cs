﻿using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Effects.Requests;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Requests;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Requests;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.Units.Effects.Systems
{
    public class EffectAttachmentRequestsProcessingSystem : AbstractUpdateSystem
    {
        private Request<EffectAttachmentRequest> _effectAttachmentRequest;
        private Request<StatChangeRequest> _statChangeRequest;
        private Request<HealthChangeProcessAdditionRequest> _healthChangeProcessAdditionRequest;
        
        [Inject] private GameEffectCatalog _gameEffectCatalog;

        public override void OnAwake()
        {
            _effectAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
            _statChangeRequest = World.GetRequest<StatChangeRequest>();
            _healthChangeProcessAdditionRequest = World.GetRequest<HealthChangeProcessAdditionRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _effectAttachmentRequest.Consume())
            {
                if (request.TargetEntity.IsNullOrDisposed() || request.EffectType == EffectType.None) continue;
                
                var effect = _gameEffectCatalog.GetData((int)request.EffectType);

                if (effect.IsChangingHealth && request.TargetEntity.Has<HealthComponent>())
                {
                    var healthChangeProcess = new HealthChangeProcess(
                        effect.HealthChangeData, 
                        effect.HealthChangeOperationType, 
                        effect.Duration,
                        effect.HealthChangeInterval);
                    
                    _healthChangeProcessAdditionRequest.Publish(new HealthChangeProcessAdditionRequest
                    {
                        TargetEntity = request.TargetEntity,
                        Process = healthChangeProcess
                    });
                }

                foreach (var change in effect.Changes)
                {
                    var statModifier = effect.IsPersist
                        ? new StatModifier(change.OperationType, change.Value)
                        : new StatModifier(change.OperationType, change.Value, effect.Duration);
                    
                    _statChangeRequest.Publish(new StatChangeRequest
                    {
                        Entity = request.TargetEntity,
                        Type = change.StatType,
                        Modifier = statModifier
                    });
                }
            }
        }
    }
}