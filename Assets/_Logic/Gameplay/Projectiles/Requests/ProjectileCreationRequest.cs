using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Projectiles.Requests
{
    public struct ProjectileCreationRequest : IRequestData
    {
        public Entity OwnerEntity;
        public Entity TargetEntity;
        public ProjectileType Type;
        public Vector3 InitialPosition;
        public Vector3 TargetPosition;
        public bool IsOriginal;
    }
}