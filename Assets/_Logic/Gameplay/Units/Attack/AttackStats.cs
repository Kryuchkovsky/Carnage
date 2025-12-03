using System;
using _Logic.Gameplay.Projectiles;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack
{
    [Serializable]
    public class AttackStats : IStatGroup
    {
        [field: SerializeField] public Stat AttackTime { get; private set; } = new(StatType.AttackDamage, 1.7f);
        [field: SerializeField] public Stat Damage { get; private set; } = new(StatType.AttackDamage, 10);
        [field: SerializeField] public Stat Range { get; private set; } = new(StatType.AttackRange, 1.5f);
        [field: SerializeField] public Stat Speed { get; private set; } = new(StatType.AttackSpeed, 100);
        [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
        [field: SerializeField] public HealthChangeType HealthChangeType { get; private set; }

        public AttackStats()
        {
        }

        public AttackStats(float damage, float basicAttackTime, float range, float speed, 
            HealthChangeType healthChangeType, ProjectileType projectileType = ProjectileType.None)
        {
            Damage = new Stat(StatType.AttackDamage, damage);
            AttackTime = new Stat(StatType.AttackTime, basicAttackTime);
            Range = new Stat(StatType.AttackRange, range);
            Speed = new Stat(StatType.AttackSpeed, speed);
            HealthChangeType = healthChangeType;
            ProjectileType = projectileType;
        }

        public StatGroupType Type => StatGroupType.AttackStats;

        public IStatGroup GetCopy()
        {
            return new AttackStats(Damage.BaseValue, AttackTime.BaseValue, Range.BaseValue, Speed.BaseValue,
                HealthChangeType, ProjectileType);
        }
    }
}