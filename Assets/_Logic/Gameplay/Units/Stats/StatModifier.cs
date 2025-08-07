namespace _Logic.Gameplay.Units.Stats
{
    public struct StatModifier
    {
        public readonly StatModifierOperationType OperationType;
        public readonly float ModifierValue;
        public readonly float Duration;
        public readonly bool IsPersist;

        public float TimeBeforeRemoving { get; private set; }

        public StatModifier(StatModifierOperationType operationType, float modifierValue, float duration)
        {
            OperationType = operationType;
            ModifierValue = modifierValue;
            Duration = duration;
            IsPersist = false;
            TimeBeforeRemoving = Duration;
        }
        
        public StatModifier(StatModifierOperationType operationType, float modifierValue)
        {
            OperationType = operationType;
            ModifierValue = modifierValue;
            Duration = float.MaxValue;
            IsPersist = true;
            TimeBeforeRemoving = Duration;
        }

        public void UpdateTime(float delta)
        {
            if (IsPersist) 
                return;
            
            TimeBeforeRemoving -= delta;
        }
    }
}