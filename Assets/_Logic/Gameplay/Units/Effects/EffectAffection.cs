using System;
using _Logic.Extensions.Attributes;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects
{
    [Serializable]
    public struct StatChange
    {
        [field: SerializeField] public StatType Type { get; private set; }
        [field: SerializeField] public StatModifierOperationType OperationType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}