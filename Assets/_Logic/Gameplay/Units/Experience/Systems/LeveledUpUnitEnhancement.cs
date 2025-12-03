using _Logic.Gameplay.Units.Stats;
using Scellecs.Morpeh.Collections;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    public class LeveledUpUnitEnhancement : IStatBuff
    {
        public FastList<StatModifier> StatModifiers { get; private set; }
        
        public StatModificationType StatModificationType => StatModificationType.Addition;
        public float Duration => -1;
        public bool IsPersist => true;

        public LeveledUpUnitEnhancement()
        {
            StatModifiers = new FastList<StatModifier>();
        }

        public void AddModifier(StatModifier statModifier)
        {
            StatModifiers.Add(statModifier);
        }
        
        public void AddModifier(FastList<StatModifier> statModifiers)
        {
            StatModifiers.AddRange(statModifiers);
        }
    }
}