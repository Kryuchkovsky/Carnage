using System.Collections.Generic;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Spawn.Components
{
    public struct UnitCounterComponent : IComponent
    {
        public Dictionary<int, int> TeamUnitNumbers;
    }
}