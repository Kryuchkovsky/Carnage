using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Spawn
{
    public struct UnitSpawnEvent : IEventData
    {
        public UnitProvider UnitProvider;
        public UnitSpawnRequest Data;
    }
}