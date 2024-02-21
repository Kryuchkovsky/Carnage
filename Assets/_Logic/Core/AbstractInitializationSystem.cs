﻿using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Core
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class AbstractInitializationSystem : IInitializer
    {
        public World World { get; set; }
        public abstract void OnAwake();

        public virtual void Dispose()
        {
        }
    }
}