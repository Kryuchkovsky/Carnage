using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Spawn;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Camera.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GameCameraTargetGroupHandlingSystem : AbstractSystem
    {
        private FilterBuilder _gameCameraFilter;
        
        public override void OnAwake()
        {
            _gameCameraFilter = World.Filter.With<GameCameraComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var spawnEvent in World.GetEvent<UnitSpawnEvent>().publishedChanges) 
            {
                if (spawnEvent.Data.HasAI) continue;
                
                foreach (var entity in _gameCameraFilter.Build())
                {
                    ref var gameCameraComponent = ref entity.GetComponent<GameCameraComponent>();
                    gameCameraComponent.Value.TargetGroup.AddMember(spawnEvent.UnitProvider.transform, 1, 1);
                }
            }
        }
    }
}