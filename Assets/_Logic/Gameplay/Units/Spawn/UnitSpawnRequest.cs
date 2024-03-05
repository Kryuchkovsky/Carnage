using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn
{
    public struct UnitSpawnRequest : IRequestData
    {
        public UnitType UnitType;
        public Vector3 Position;
        public int TeamId;
        public int Priority;
        public bool IsPrioritizedTarget;
        public bool HasAI;
    }
}