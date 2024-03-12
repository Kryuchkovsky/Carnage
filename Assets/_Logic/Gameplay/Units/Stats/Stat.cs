using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Logic.Gameplay.Units.Stats
{
    [Serializable]
    public class Stat
    {
        private readonly List<StatModifier> _modifiers;

        [field: SerializeField] public float BaseValue { get; private set; }
        
        public float CurrentValue { get; set; }

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

        public void UpdateModifiers(float delta)
        {
            for (int i = 0; i < _modifiers.Count;)
            {
                _modifiers[i].UpdateTime(delta);

                if (_modifiers[i].TimeBeforeRemoving <= 0)
                {
                    RemoveModifier(i);
                }
                else
                {
                    i++;
                }
            }
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
                switch (modifier.OperationType)
                {
                    case StatModifierOperationType.Addition:
                        CurrentValue += modifier.ModifierValue;
                        break;
                    case StatModifierOperationType.Multiplication:
                        CurrentValue += BaseValue * modifier.ModifierValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}