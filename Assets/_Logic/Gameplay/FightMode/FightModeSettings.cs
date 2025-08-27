using System.Collections.Generic;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.FightMode
{
    [CreateAssetMenu(menuName = "Create FightModeSettings", fileName = "FightModeSettings", order = 1)]
    public class FightModeSettings : Config
    {
        [field: SerializeField] public List<UnitType> Heroes { get; private set; }
        [field: SerializeField] public List<UnitType> Units { get; private set; }
        [field: SerializeField] public EffectType PlayerEnhancmentEffectType { get; private set; }
        [field: SerializeField, Range(0, 100)] public int MaxUnitsInArmy { get; private set; } = 10;
        [field: SerializeField] public bool SpawnPlayer { get; private set; }
        
        public UnitType GetRandomUnit()
        {
            return Units[Random.Range(0, Units.Count)];
        }
    }
}