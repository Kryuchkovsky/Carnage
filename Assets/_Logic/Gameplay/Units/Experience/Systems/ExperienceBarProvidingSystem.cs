using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    public sealed class ExperienceBarProvidingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<ExperienceComponent>().With<AliveComponent>()
                .Without<ExperienceBarComponent>().Without<AIComponent>()
                .ForEach((Entity entity) =>
                {
                    entity.SetComponent(new ExperienceBarComponent
                    {
                        Value = PlayerExperienceBar.Instance
                    });
                });
        }
    }
}