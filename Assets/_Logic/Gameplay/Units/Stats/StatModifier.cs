using System;
using UnityEngine;

namespace _Logic.Gameplay.Units.Stats
{
    [Serializable]
    public struct StatModifier
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public StatModifierOperationType OperationType { get; private set; }
        [field: SerializeField] public float Value { get; set; }

        public StatModifier(StatType statType, StatModifierOperationType operationType, float value)
        {
            StatType = statType;
            OperationType = operationType;
            Value = value;
        }
    }
}