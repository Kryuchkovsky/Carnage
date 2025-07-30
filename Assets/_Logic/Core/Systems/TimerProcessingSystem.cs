using System;
using _Logic.Core.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Core.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class TimerProcessingSystem : AbstractUpdateSystem
    {
        private Filter _timerFilter;
        private Stash<TimerComponent> _timerStash;
        
        public override void OnAwake()
        {
            _timerFilter = World.Filter.With<TimerComponent>().Build();
            _timerStash = World.GetStash<TimerComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _timerFilter)
            {
                ref var timerComponent = ref _timerStash.Get(entity);
                
                if (timerComponent.Value > 0)
                {
                    timerComponent.Value -= deltaTime;
                }
            }
        }
    }
}