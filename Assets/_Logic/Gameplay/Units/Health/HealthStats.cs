using System;
using UnityEngine;

namespace _Logic.Gameplay.Units.Health
{
    [Serializable]
    public class HealthStats : IUnitStats
    {
        [field: SerializeField] public Stat MaxHealth { get; private set; } = new(100);
        [field: SerializeField] public Stat RegenerationRate { get; private set; } = new(1);
        [field: SerializeField] public Stat CorpseExistenceTime { get; private set; } = new(3);

        public HealthStats()
        {
        }

        public HealthStats(float maxHealth, float regenerationRate, float corpseExistenceTime)
        {
            MaxHealth = new Stat(maxHealth);
            RegenerationRate = new Stat(regenerationRate);
            CorpseExistenceTime = new Stat(corpseExistenceTime);
        }

        public IUnitStats GetCopy() => new HealthStats(MaxHealth.BaseValue, RegenerationRate.BaseValue, CorpseExistenceTime.BaseValue);
    }
}