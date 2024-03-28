using _Logic.Core;
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
        
        public override void OnAwake()
        {
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _effectAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var @event in _unitSpawnEvent.publishedChanges) 
            {
                if (@event.Entity.Has<AIComponent>() || !@event.Entity.Has<UnitComponent>()) continue;
                
                _effectAttachmentRequest.Publish(new EffectAttachmentRequest
                {
                    TargetEntity = @event.Entity,
                    EffectType = _survivalModeSettings.PlayerEnhancmentEffectType
                }, true);
                
                @event.Entity.SetComponent(new SplitAttackComponent
                {
                    AdditionalTargets = 3
                });
            }
        }
    }
}