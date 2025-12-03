using System;
using UnityEngine;

namespace _Logic.Gameplay.Units.Stats
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public float BaseValue { get; private set; }
        
        public float CurrentValue => BaseValue + Change;
        public float Change { get; set; }

        public Stat(StatType statType, float baseValue)
        {
            StatType = statType;
            BaseValue = baseValue;
        }

        public void Reset()
        {
            Change = 0;
        }
    }
}