using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Experience.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ExperienceComponent : IComponent
    {
        public int Level;
        public float TotalExperienceAmount;
        public float CurrentLevelCost;
        public float NextLevelCost;
        public float LevelUpCost;
        public float Progress;
    }
}