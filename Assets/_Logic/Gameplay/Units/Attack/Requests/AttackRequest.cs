using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Requests
{
    public struct AttackRequest : IRequestData
    {
        public Entity AttackerEntity;
        public Entity TargetEntity;
        public Vector3 AttackPosition;
        public bool IsOriginal;
    }
}