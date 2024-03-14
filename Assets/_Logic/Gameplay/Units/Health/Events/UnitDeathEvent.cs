using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health.Events
{
    public struct UnitDeathEvent : IEventData
    {
        public Entity CorpseEntity;
        public Entity MurdererEntity;
    }
}
