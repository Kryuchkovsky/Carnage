using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
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