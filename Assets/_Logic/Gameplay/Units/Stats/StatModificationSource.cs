using System;
using System.Collections.Generic;

namespace _Logic.Gameplay.Units.Stats
{
    public class StatModificationSource
    {
        private IStatBuff _statBuff;
        
        public Dictionary<StatType, StatModifier> Modifiers { get; private set; } = new();

        public float TimeBeforeRemoving { get; private set; }

        public StatModificationSource(IStatBuff statBuff)
        {
            _statBuff = statBuff;
            AddBuff(statBuff);
        }

        public void AddBuff(IStatBuff statBuff)
        {
            foreach (var modifier in statBuff.StatModifiers)
            {
                if (Modifiers.TryGetValue(modifier.StatType, out var existedModifier))
                {
                    switch (statBuff.StatModificationType)
                    {
                        case StatModificationType.Addition:
                            existedModifier.Value = modifier.Value;
                            break;
                        case StatModificationType.Replacement:
                            existedModifier.Value += modifier.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Modifiers[modifier.StatType] = existedModifier;
                }
                else Modifiers.Add(modifier.StatType, new StatModifier(modifier.StatType, modifier.OperationType, modifier.Value));
            }

            TimeBeforeRemoving += statBuff.Duration;
        }

        public void Update(float deltaTime)
        {
            if (_statBuff.IsPersist) 
                return;
            
            TimeBeforeRemoving -= deltaTime;
        }

        public void Apply(Stat stat)
        {
            if ((TimeBeforeRemoving >= 0 || _statBuff.IsPersist) &&
                Modifiers.TryGetValue(stat.StatType, out var statModifier))
            {
                switch (statModifier.OperationType)
                {
                    case StatModifierOperationType.Addition:
                        stat.Change += statModifier.Value;
                        break;
                    case StatModifierOperationType.Multiplication:
                        stat.Change += stat.BaseValue * statModifier.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}