using _Logic.Core;
using _Logic.Gameplay.Items;
using _Logic.Gameplay.Items.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Effects.Requests;
using _Logic.Gameplay.Units.Spawn;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SurvivalModeEnhancementSystem : AbstractUpdateSystem
    {
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private Request<EffectAttachmentRequest> _effectAttachmentRequest;
        
        [Inject] private SurvivalModeSettings _survivalModeSettings;
        [Inject] private ItemConfig _itemConfig;
        
        public override void OnAwake()
        {
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _effectAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _unitSpawnEvent.publishedChanges) 
            {
                if (evt.Entity.Has<AIComponent>() || !evt.Entity.Has<UnitComponent>()) continue;
                
                _effectAttachmentRequest.Publish(new EffectAttachmentRequest
                {
                    TargetEntity = evt.Entity,
                    EffectType = _survivalModeSettings.PlayerEnhancmentEffectType
                }, true);
                
                evt.Entity.SetComponent(new SplitAttackComponent
                {
                    AdditionalTargets = 3
                });
                
                evt.Entity.SetComponent(new CollectorComponent
                {
                    Radius = _itemConfig.CollectionRange
                });
            }
        }
    }
}