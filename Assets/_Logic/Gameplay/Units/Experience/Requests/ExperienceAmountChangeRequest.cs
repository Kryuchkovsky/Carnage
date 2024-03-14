using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Experience.Requests
{
    public struct ExperienceAmountChangeRequest : IRequestData
    {
        public Entity ReceivingEntity;
        public float Change;
    }
}