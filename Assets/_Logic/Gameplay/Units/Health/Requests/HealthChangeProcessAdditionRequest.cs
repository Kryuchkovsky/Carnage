using System;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health.Requests
{
    [Serializable]
    public struct HealthChangeProcessAdditionRequest : IRequestData
    {
        public Entity TargetEntity;
        public HealthChangeProcess Process;
    }
}