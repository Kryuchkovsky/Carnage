using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack.Events
{
    public struct DamageEvent : IEventData
    {
        public Entity ReceiverEntity;
        public float Damage;
    }
}