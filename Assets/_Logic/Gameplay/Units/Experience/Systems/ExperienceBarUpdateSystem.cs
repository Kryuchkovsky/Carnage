using _Logic.Core;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExperienceBarUpdateSystem : AbstractUpdateSystem
    {
        private Event<ExperienceAmountChangeEvent> _experienceAmountChangeEvent;
        private Event<LevelChangeEvent> _levelChangeEvent;
        
        [Inject] private ExperienceSettings _experienceSettings;
        
        public override void OnAwake()
        {
            _experienceAmountChangeEvent = World.GetEvent<ExperienceAmountChangeEvent>();
            _levelChangeEvent = World.GetEvent<LevelChangeEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _experienceAmountChangeEvent.publishedChanges)
            {
                UpdateExperienceBar(evt.ReceivingEntity);
            }

            foreach (var evt in _levelChangeEvent.publishedChanges)
            {
                UpdateExperienceBar(evt.Entity);
            }
        }

        private void UpdateExperienceBar(Entity entity)
        {
            ref var expComponent = ref entity.GetComponent<ExperienceComponent>(out var hasExpComponent);
            ref var expBarComponent = ref entity.GetComponent<ExperienceBarComponent>(out var hasExperienceBarComponent);
                
            if (hasExpComponent && hasExperienceBarComponent)
            {
                expBarComponent.Value.SetLevel(expComponent.Level);
                expBarComponent.Value.SetFilling(expComponent.Progress);
                expBarComponent.Value.SetExperienceAmount(expComponent.TotalExperienceAmount - expComponent.CurrentLevelCost, expComponent.NextLevelCost);
            }
        }
    }
}