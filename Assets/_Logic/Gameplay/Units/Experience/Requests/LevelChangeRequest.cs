using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Experience.Requests
{
    public struct LevelChangeRequest : IRequestData
    {
        public Entity Entity;
        public int Change;
    }
}