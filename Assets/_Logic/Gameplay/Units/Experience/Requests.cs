using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Experience
{
    public struct ExperienceAmountChangeRequest : IRequestData
    {
        public Entity ReceivingEntity;
        public float Change;
    }
    
    public struct LevelChangeRequest : IRequestData
    {
        public Entity Entity;
        public int Change;
    }
}