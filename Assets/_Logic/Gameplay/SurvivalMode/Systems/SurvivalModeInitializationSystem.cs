using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.SurvivalMode.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SurvivalModeInitializationSystem : AbstractInitializationSystem
    {
        public override void OnAwake()
        {
            var entity = World.CreateEntity();
            entity.AddComponent<SurvivalModeComponent>();
            entity.AddComponent<TimerComponent>();
        }
    }
}