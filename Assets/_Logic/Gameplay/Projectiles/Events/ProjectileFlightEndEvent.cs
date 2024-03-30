using Scellecs.Morpeh;

namespace _Logic.Gameplay.Projectiles.Events
{
    public struct ProjectileFlightEndEvent : IEventData
    {
        public Entity OwnerEntity;
        public Entity TargetEntity;
        public Entity ProjectileEntity;
    }
}