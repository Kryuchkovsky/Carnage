using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Movement
{
    public struct DestinationChangeRequest : IRequestData
    {
        public Entity Entity;
        public Vector3 Destination;
    }
}