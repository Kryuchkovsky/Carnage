using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.SurvivalMode.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SurvivalModeComponent : IComponent
    {
        public int HeroId;
    }
}