using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Creatures.Components
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct CreatureComponent : IComponent
    {
        public CreatureProvider Value;
    }
}