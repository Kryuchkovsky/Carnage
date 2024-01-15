using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Movement.Components
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public struct MovementComponent : IComponent
    {
        public MovementData BacisData;
        public MovementData CurrentData;
    }
}