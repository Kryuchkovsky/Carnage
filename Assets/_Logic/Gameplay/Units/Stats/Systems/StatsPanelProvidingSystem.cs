using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Stats.Systems
{
    public sealed class StatsPanelProvidingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<StatsComponent>().With<AliveComponent>()
                .Without<StatsPanelComponent>().Without<AIComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref StatsComponent statsComponent) =>
                {
                    entity.SetComponent(new StatsPanelComponent
                    {
                        Value = StatsPanel.Instance
                    });

                    StatsPanel.Instance.Initiate(statsComponent.Value.Stats);
                });
        }
    }
}