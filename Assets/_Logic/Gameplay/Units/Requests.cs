using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public struct UnitSpawnRequest : IRequestData
    {
        public Vector3 Position;
        public string UnitId;
        public int TeamId;
    }
}