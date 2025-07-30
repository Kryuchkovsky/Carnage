using _Logic.Core;
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
    public sealed class ExperienceBarProvidingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<ExperienceBarComponent> _experienceBarStash;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<ExperienceComponent>().With<AliveComponent>()
                .Without<ExperienceBarComponent>().Without<AIComponent>().Build();
            _experienceBarStash = World.GetStash<ExperienceBarComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                _experienceBarStash.Set(entity, new ExperienceBarComponent
                {
                    Value = PlayerExperienceBar.Instance
                });
            }
        }
    }
}