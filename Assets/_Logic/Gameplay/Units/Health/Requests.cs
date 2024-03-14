﻿using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health
{
    public struct HealthChangeRequest : IRequestData
    {
        public Entity TargetEntity;
        public Entity SenderEntity;
        public float Change;
    }
}