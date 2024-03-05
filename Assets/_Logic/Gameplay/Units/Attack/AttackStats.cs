using System;
using _Logic.Gameplay.Units.Projectiles;
using JetBrains.Annotations;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack
{
    [Serializable]
    public class AttackStats : IUnitStats
    {
        [field: SerializeField] public Stat Damage { get; private set; } = new(10);
        [field: SerializeField] public Stat BasicAttackTime { get; private set; } = new(1.7f);
        [field: SerializeField] public Stat Range { get; private set; } = new(1.5f);
        [field: SerializeField] public Stat Speed { get; private set; } = new(100);
        [field: SerializeField, CanBeNull] public ProjectileData ProjectileData { get; private set; }

        public AttackStats()
        {
        }

        public AttackStats(float damage, float basicAttackTime, float range, float speed, ProjectileData projectileData = null)
        {
            Damage = new Stat(damage);
            BasicAttackTime = new Stat(basicAttackTime);
            Range = new Stat(range);
            Speed = new Stat(speed);
            ProjectileData = projectileData;
        }

        public IUnitStats GetCopy() => new AttackStats(Damage.BaseValue, BasicAttackTime.BaseValue, Range.BaseValue, Speed.BaseValue, ProjectileData);
    }
}