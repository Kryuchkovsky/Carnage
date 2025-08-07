using _Logic.Core;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.Units.Stats.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class StatUpdateSystem : AbstractUpdateSystem
    {
        private Filter _statsFilter;
        private Stash<StatsComponent> _statsStash;
        
        [Inject] private StatsPanel _statsPanel;

        public override void OnAwake()
        {
            _statsFilter = World.Filter.With<StatsComponent>().Build();
            _statsStash = World.GetStash<StatsComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _statsFilter)
            {
                ref var statsComponent = ref _statsStash.Get(entity);

                if (statsComponent.Value.HasChangedStat)
                    _statsPanel.Update();

                statsComponent.Value.Update(deltaTime);
            }
        }
    }
}