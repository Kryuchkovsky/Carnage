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
            foreach (var spawnEvent in _unitSpawnEvent.publishedChanges) 
            {
                if (spawnEvent.Entity.Has<AIComponent>() || !spawnEvent.Entity.Has<TransformComponent>()) continue;

                var transform = spawnEvent.Entity.GetComponent<TransformComponent>().Value;
                
                foreach (var entity in _gameCameraFilter.Build())
                {
                    ref var gameCameraComponent = ref entity.GetComponent<GameCameraComponent>();
                    gameCameraComponent.Value.TargetGroup.AddMember(transform, 1, 1);
                }
            }
        }
    }
}