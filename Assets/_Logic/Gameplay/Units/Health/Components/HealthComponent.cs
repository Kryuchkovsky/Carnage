using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Health.Components
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct HealthComponent : IComponent
    {
        public List<HealthChangeProcess> PeriodicHealthChanges;
        public HealthStats Stats;
        public float CurrentHealth;
        public float Percentage;
        public bool IsDead;
    }
}
