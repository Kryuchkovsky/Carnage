using System.Collections.Generic;
using _Logic.Gameplay.Units.Stats;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    public class LeveledUpUnitEnhancement : IStatBuff
    {
        public List<StatModifier> StatModifiers { get; private set; }
        
        public StatModificationType StatModificationType => StatModificationType.Addition;
        public float Duration => -1;
        public bool IsPersist => true;

        public LeveledUpUnitEnhancement()
        {
            StatModifiers = new List<StatModifier>();
        }

        public void AddModifier(StatModifier statModifier)
        {
            StatModifiers.Add(statModifier);
        }
        
        public void AddModifier(List<StatModifier> statModifiers)
        {
            StatModifiers.AddRange(statModifiers);
        }
    }
}