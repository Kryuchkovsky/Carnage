using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _GameLogic.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class AbstractInitializer : IInitializer
    {
        public World World { get; set; }
        public abstract void OnAwake();

        public virtual void Dispose()
        {
        }
    }
}