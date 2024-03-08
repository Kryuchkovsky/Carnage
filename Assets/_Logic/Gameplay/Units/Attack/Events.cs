using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack
{
    public struct AttackAnimationCompletionEvent : IEventData
    {
        public Entity Entity;
    }
    
    public struct DamageEvent : IEventData
    {
        public Entity ReceiverEntity;
        public float Damage;
    }
}