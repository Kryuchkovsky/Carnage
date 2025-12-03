using System;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units.Health
{
    [Serializable]
    public class HealthStats : IStatGroup
    {
        [field: SerializeField] public Stat MaxHealth { get; private set; } = new(StatType.MaxHeath, 100);
        [field: SerializeField] public Stat RegenerationRate { get; private set; } = new(StatType.HealthRegenerationRate, 0.5f);

        public HealthStats()
        {
        }

        public HealthStats(float maxHealth, float regenerationRate)
        {
            MaxHealth = new Stat(StatType.MaxHeath, maxHealth);
            RegenerationRate = new Stat(StatType.HealthRegenerationRate, regenerationRate);
        }

        public StatGroupType Type => StatGroupType.HealthStats;

        public IStatGroup GetCopy() => new HealthStats(MaxHealth.BaseValue, RegenerationRate.BaseValue);
    }
}