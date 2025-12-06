using System.Collections.Generic;

namespace _Logic.Gameplay.Units.Stats
{
    public interface IStatBuff
    {
        public List<StatModifier> StatModifiers { get; }
        public StatModificationType StatModificationType { get; }
        public bool IsPersist { get; }
        public float Duration { get;}
    }
}