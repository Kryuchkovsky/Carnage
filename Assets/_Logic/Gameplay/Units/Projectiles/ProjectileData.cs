using System;
using UnityEngine;

namespace _Logic.Gameplay.Units.Projectiles
{
    [Serializable]
    public struct ProjectileData
    {
        public string Id;
        public float Speed;
        [Range(0, 1)] public float FlightRangeToHeightRatio;
    }
}