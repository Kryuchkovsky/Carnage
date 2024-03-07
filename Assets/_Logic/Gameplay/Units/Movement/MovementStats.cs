﻿using System;
using UnityEngine;

namespace _Logic.Gameplay.Units.Movement
{
    [Serializable]
    public class MovementStats : IUnitStats
    {
        [field: SerializeField] public Stat MovementSpeed { get; private set; } = new(10);
        [field: SerializeField] public Stat RotationSpeed { get; private set; } = new(360);

        public MovementStats()
        {
        }

        public MovementStats(float movementSpeed, float rotationSpeed)
        {
            MovementSpeed = new Stat(movementSpeed);
            RotationSpeed = new Stat(rotationSpeed);
        }

        public IUnitStats GetCopy() => new MovementStats(MovementSpeed.BaseValue, RotationSpeed.BaseValue);
    }
}