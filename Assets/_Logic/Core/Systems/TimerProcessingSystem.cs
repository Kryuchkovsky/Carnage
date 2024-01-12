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
                .ForEach((Entity entity, ref TimerComponent timer) =>
                {
                    if (timer.Value > 0)
                    {
                        timer.Value -= deltaTime;
                    }
                });
        }
    }
}