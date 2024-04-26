using _Logic.Core.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Core.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class TimerProcessingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<TimerComponent>()
                .ForEach((Entity entity, ref TimerComponent timerComponent) =>
                {
                    if (timerComponent.Value > 0)
                    {
                        timerComponent.Value -= deltaTime;
                    }
                });
        }
    }
}