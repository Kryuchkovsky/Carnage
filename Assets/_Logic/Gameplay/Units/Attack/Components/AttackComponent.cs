using System.Collections.Generic;
using _Logic.Gameplay.Units.Health;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Attack.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AttackComponent : IComponent
    {
        public HashSet<ImpactType> ImpactTypes;
        public ProjectileType ProjectileType;
        public HealthChangeType AttackHealthChangeType;
        public float AttacksPerSecond;
        public float AttackTime;
        public float AttackTimePercentage;
        public float RemainingAttackTime;
        public bool IsOriginal;
    }
}