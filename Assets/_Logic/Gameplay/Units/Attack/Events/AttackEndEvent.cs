using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack.Events
{
    public struct AttackEndEvent : IEventData
    {
        public Entity AttackerEntity;
        public Entity TargetEntity;
    }
}