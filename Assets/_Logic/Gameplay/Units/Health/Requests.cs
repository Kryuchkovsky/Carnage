using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health
{
    public struct HealthChangeRequest : IRequestData
    {
        public Entity Entity;
        public float Change;
    }
}