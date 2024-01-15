using System;

namespace _Logic.Gameplay.Units.Attack
{
    [Serializable]
    public struct AttackData
    {
        public float BasicAttackTime;
        public int Damage;
        public float Range;
        public float Speed;
    }
}