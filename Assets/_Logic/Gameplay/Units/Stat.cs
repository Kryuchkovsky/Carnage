using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    [Serializable]
    public class Stat
    {
        private readonly List<StatModifier> _modifiers;

        [field: SerializeField] public float BaseValue { get; private set; }
        
        public float CurrentValue { get; private set; }

        public Stat(float baseValue)
        {
            BaseValue = baseValue;
            _modifiers = new List<StatModifier>();
            CurrentValue = baseValue;
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            RecalculateStatValue();
        }

        public void RemoveModifier(int index)
        {
            _modifiers.RemoveAt(index);
            RecalculateStatValue();
        }

        private void RecalculateStatValue()
        {
            CurrentValue = BaseValue;
            
            foreach (var modifier in _modifiers)
            {
                switch (modifier.ActionType)
                {
                    case StatModifierActionType.Addition:
                        CurrentValue += modifier.ModifierValue;
                        break;
                    case StatModifierActionType.Multiplication:
                        CurrentValue *= modifier.ModifierValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public struct StatModifier
    {
        public readonly StatModifierActionType ActionType;
        public readonly float ModifierValue;

        public StatModifier(float modifierValue, StatModifierActionType actionType)
        {
            ModifierValue = modifierValue;
            ActionType = actionType;
        }
    }

    public enum StatModifierActionType
    {
        Addition,
        Multiplication
    }
}