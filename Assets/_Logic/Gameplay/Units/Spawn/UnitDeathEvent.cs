using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Spawn
{
    public struct UnitDeathEvent : IEventData
    {
        public UnitProvider UnitProvider;
        public UnitSpawnRequest Data;
    }
}