using System;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units.Movement
{
    [Serializable]
    public class MovementStats : IStatGroup
    {
        [field: SerializeField] public Stat MovementSpeed { get; private set; } = new(StatType.MovementSpeed, 10);
        [field: SerializeField] public Stat RotationSpeed { get; private set; } = new(StatType.RotationSpeed, 360);

        public MovementStats()
        {
        }

        public MovementStats(float movementSpeed, float rotationSpeed)
        {
            MovementSpeed = new Stat(StatType.MovementSpeed, movementSpeed);
            RotationSpeed = new Stat(StatType.RotationSpeed, rotationSpeed);
        }

        public StatGroupType Type => StatGroupType.MovementStats;

        public IStatGroup GetCopy() => new MovementStats(MovementSpeed.BaseValue, RotationSpeed.BaseValue);
    }
}