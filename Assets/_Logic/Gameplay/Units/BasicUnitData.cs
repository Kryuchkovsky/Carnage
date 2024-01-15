using System;
using _Logic.Gameplay.Units.Attack;
using _Logic.Gameplay.Units.Creatures;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    [Serializable]
    public class BasicUnitData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public CreatureProvider Prefab { get; private set; }
        [field: SerializeField, Min(0)] public float SpawnTime { get; private set; } = 3f;

        [field: SerializeField]
        public AttackData AttackData { get; private set; } = new()
        {
            BasicAttackTime = 2,
            Damage = 10,
            Range = 1,
            Speed = 100
        };

        [field: SerializeField]
        public HealthData HealthData { get; private set; } = new()
        {
            MaxValue = 100
        };

        [field: SerializeField]
        public MovementData MovementData { get; private set; } = new()
        {
            MovementSpeed = 4,
            RotationSpeed = 720
        };
    }
}