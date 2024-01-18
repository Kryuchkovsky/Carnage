using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Buildings.Components
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [System.Serializable]
    public struct BarrackComponent : IComponent
    {
        public BarrackProvider Value;
    }
}