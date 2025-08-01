﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Logic.Gameplay.Units.Stats
{
    [Serializable]
    public class Stat
    {
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField] public float CurrentValue { get; private set; }

        public List<StatModifier> Modifiers { get; private set; }
        
        public bool IsChangedInLastUpdate { get; private set; }

        public Stat(float baseValue)
        {
            BaseValue = baseValue;
            Modifiers = new List<StatModifier>();
            CurrentValue = baseValue;
        }

        public void AddModifier(StatModifier modifier)
        {
            Modifiers.Add(modifier);

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

            IsChangedInLastUpdate = true;
        }

        public void RemoveModifier(int index)
        {
            switch (Modifiers[index].OperationType)
            {
                case StatModifierOperationType.Addition:
                    CurrentValue -= Modifiers[index].ModifierValue;
                    break;
                case StatModifierOperationType.Multiplication:
                    CurrentValue -= BaseValue * Modifiers[index].ModifierValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Modifiers.RemoveAt(index);
            IsChangedInLastUpdate = true;
        }

        public void Update(float deltaTime)
        {
            IsChangedInLastUpdate = false;
            
            for (int i = 0; i < Modifiers.Count;)
            {
                if (Modifiers[i].IsPersist)
                {
                    i++;
                }
                else
                {
                    Modifiers[i].UpdateTime(deltaTime);
                            
                    if (Modifiers[i].TimeBeforeRemoving <= 0)
                    {
                        RemoveModifier(i);
                    }
                }
            }
        }

        public void Reset()
        {
            Modifiers.Clear();
            RecalculateStatValue();
        }

        private void RecalculateStatValue()
        {
            CurrentValue = BaseValue;

            foreach (var modifier in Modifiers)
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