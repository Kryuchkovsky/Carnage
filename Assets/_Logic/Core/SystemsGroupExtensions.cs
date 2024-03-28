#if MORPEH

using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _GameLogic.Extensions
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class SystemsGroupExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddInitializer<T>(this SystemsGroup systemsGroup, T initializer, IObjectResolver resolver) where T : class, IInitializer
        {
            resolver.Inject(initializer);
            systemsGroup.AddInitializer(initializer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddSystem<T>(this SystemsGroup systemsGroup, T system, IObjectResolver resolver, bool enabled = true) where T : class, ISystem
        {
            resolver.Inject(system);
            return systemsGroup.AddSystem(system, enabled);
        }
    }
}
#endif