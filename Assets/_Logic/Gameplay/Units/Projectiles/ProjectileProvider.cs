using System;
using _Logic.Core;
using _Logic.Gameplay.Units.Projectiles.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Projectiles
{
    public class ProjectileProvider : GameObjectProvider<ProjectileComponent>
    {
        public event Action<ProjectileProvider> FlightEnded;
        
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new ProjectileComponent
            {
                Value = this
            });
        }

        public void OnFlightEnded()
        {
            FlightEnded?.Invoke(this);
            FlightEnded = null;
        }
    }
}