using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Effects;
using _Logic.Gameplay.Units.Effects.Requests;
using _Logic.Gameplay.Units.Spawn;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SurvivalModeEnhancementSystem : AbstractUpdateSystem
    {
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private Request<EffectAttachmentRequest> _effectAttachmentRequest;
        private GameEffectCatalog _gameEffectCatalog;
        private SurvivalModeSettings _survivalModeSettings;
        
        public override void OnAwake()
        {
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _effectAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
            _gameEffectCatalog = ConfigManager.Instance.GetConfig<GameEffectCatalog>();
            _survivalModeSettings = ConfigManager.Instance.GetConfig<SurvivalModeSettings>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var spawnEvent in _unitSpawnEvent.publishedChanges) 
            {
                if (spawnEvent.Entity.Has<AIComponent>() || !spawnEvent.Entity.Has<TransformComponent>()) continue;
                
                _effectAttachmentRequest.Publish(new EffectAttachmentRequest
                {
                    TargetEntity = spawnEvent.Entity,
                    EffectType = _survivalModeSettings.PlayerEnhancmentEffectType
                }, true);
            }
        }
    }
}