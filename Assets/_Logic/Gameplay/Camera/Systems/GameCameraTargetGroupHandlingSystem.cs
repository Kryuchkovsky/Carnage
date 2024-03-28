using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Spawn;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Camera.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GameCameraTargetGroupHandlingSystem : AbstractUpdateSystem
    {
        private FilterBuilder _gameCameraFilter;
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        
        public override void OnAwake()
        {
            _gameCameraFilter = World.Filter.With<GameCameraComponent>();
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _unitSpawnEvent.publishedChanges) 
            {
                if (evt.Entity.Has<AIComponent>() || !evt.Entity.Has<TransformComponent>()) continue;

                var transformComponent = evt.Entity.GetComponent<TransformComponent>();
                
                foreach (var entity in _gameCameraFilter.Build())
                {
                    ref var gameCameraComponent = ref entity.GetComponent<GameCameraComponent>();
                    gameCameraComponent.Value.TargetGroup.AddMember(transformComponent.Value, 1, 1);
                }
            }
        }
    }
}