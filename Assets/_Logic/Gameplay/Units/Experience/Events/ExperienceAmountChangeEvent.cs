using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Experience.Events
{
    public struct ExperienceAmountChangeEvent : IEventData
    {
        public Entity ReceivingEntity;
        public float Change;
    }
}