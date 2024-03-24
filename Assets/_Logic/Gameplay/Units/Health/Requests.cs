﻿using System;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health
{
    [Serializable]
    public struct HealthChangeRequest : IRequestData
    {
        public Entity TargetEntity;
        public Entity SenderEntity;
        public HealthChangeData Data;
        public bool CreatePopup;
    }
}