using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Experience.Events
{
    public struct LevelChangeEvent : IEventData
    {
        public Entity Entity;
        public int NewLevel;
        public int Change;
    }
}