using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects.Requests
{
    public struct ImpactCreationRequest : IRequestData
    {
        public Entity InvokerEntity;
        public ImpactType Type;
        public Vector3 Position;
    }
}