using System.Collections.Generic;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn
{
    [System.Serializable]
    public class SpawnAbilityData : IStatGroup
    {
        [field: SerializeField] public List<UnitType> Units { get; private set; }
        [field: SerializeField] public Stat SpawnInterval { get; private set; }
        
        public StatGroupType Type => StatGroupType.SpawnStats;

        public void Update(float delta)
        {
            SpawnInterval.UpdateModifiers(delta);
        }

        public IStatGroup GetCopy() =>
            new SpawnAbilityData
            {
                SpawnInterval = new Stat(SpawnInterval.BaseValue),
                Units = new List<UnitType>(Units)
            };
    }
}