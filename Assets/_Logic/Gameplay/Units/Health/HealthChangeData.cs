using System;

namespace _Logic.Gameplay.Units.Health
{
    [Serializable]
    public struct HealthChangeData
    {
        public HealthChangeType Type;
        public float Value;
    }
}