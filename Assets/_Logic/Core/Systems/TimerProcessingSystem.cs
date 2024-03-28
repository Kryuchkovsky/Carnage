using _Logic.Core.Components;
using Scellecs.Morpeh;

namespace _Logic.Core.Systems
{
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