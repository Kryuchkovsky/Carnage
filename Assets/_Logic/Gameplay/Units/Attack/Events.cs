using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack
{
    public struct DamageEvent : IEventData
    {
        public Entity ReceiverEntity;
        public float Damage;
    }
}