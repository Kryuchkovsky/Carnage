using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Effects.Requests
{
    public struct ImpactCreationRequest : IRequestData
    {
        public Entity Invoker;
        public ImpactType Type;
        public Vector3 Position;
    }
}