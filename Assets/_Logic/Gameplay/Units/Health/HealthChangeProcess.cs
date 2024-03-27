using System;
using _Logic.Gameplay.Units.Stats;

namespace _Logic.Gameplay.Units.Health
{
    [Serializable]
    public struct HealthChangeProcess
    {
        public HealthChangeData Data;
        public StatModifierOperationType OperationType;
        public float Duration;
        public float Interval;
        public float ExistingTime;
        public float LastChangeTime;
        public bool IsPersist;

        public HealthChangeProcess(HealthChangeData data, StatModifierOperationType operationType, float duration, float interval)
        {
            Data = data;
            OperationType = operationType;
            Duration = duration;
            Interval = interval;
            IsPersist = false;
            ExistingTime = 0;
            LastChangeTime = 0;
        }
        
        public HealthChangeProcess(HealthChangeData data, StatModifierOperationType operationType, float interval)
        {
            Data = data;
            OperationType = operationType;
            Interval = interval;
            Duration = float.MaxValue;
            IsPersist = true;
            ExistingTime = 0;
            LastChangeTime = 0;
        }
    }
}