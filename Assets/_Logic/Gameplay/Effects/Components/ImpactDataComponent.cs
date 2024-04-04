using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Effects.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ImpactDataComponent : IComponent
    {
        public HashSet<Entity> ImpactedEntities;
        public ImpactData Data;
        public float LastCheckTime;
        public float Progress;
    }
}