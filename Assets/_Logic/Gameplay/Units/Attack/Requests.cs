using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack
{
    public struct DamageRequest : IRequestData
    {
        public Entity ReceiverEntity;
        public float Damage;
    }
}