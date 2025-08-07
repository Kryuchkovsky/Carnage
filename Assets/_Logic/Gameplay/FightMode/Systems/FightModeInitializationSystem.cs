using _Logic.Core;
using _Logic.Gameplay.FightMode.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.FightMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class FightModeInitializationSystem : AbstractInitializationSystem
    {
        public override void OnAwake()
        {
            var entity = World.CreateEntity();
            World.GetStash<FightModeComponent>().Add(entity);
        }
    }
}