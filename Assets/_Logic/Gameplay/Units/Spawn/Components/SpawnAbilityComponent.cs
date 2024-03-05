using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpawnAbilityComponent : IComponent
    {
        public SpawnAbilityData Data;
    }
    
    [System.Serializable]
    public class SpawnAbilityData : IUnitStats
    {
        public float SpawnSpeedMultiplier;
        
        [field: SerializeField] public List<UnitType> Units { get; private set; }
        
        public IUnitStats GetCopy() =>
            new SpawnAbilityData
            {
                SpawnSpeedMultiplier = SpawnSpeedMultiplier,
                Units = new List<UnitType>(Units)
            };
    }
}