using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Projectiles
{
    public struct DullProjectileCreationRequest : IRequestData
    {
        public ProjectileData Data;
        public Vector3 InitialPosition;
        public Vector3 EndPosition;
        public Action Callback;
    }
    
    public struct HomingProjectileCreationRequest : IRequestData
    {
        public ProjectileData Data;
        public Transform Target;
        public Vector3 InitialPosition;
        public Vector3 Offset;
        public Action<ProjectileProvider> Callback;
    }
}