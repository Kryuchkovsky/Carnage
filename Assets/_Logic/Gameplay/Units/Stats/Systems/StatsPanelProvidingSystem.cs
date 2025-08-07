using _Logic.Core;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.Units.Stats.Systems
{
    public sealed class StatsPanelProvidingSystem : AbstractInitializationSystem
    {
        private Filter _filter;
        private Stash<StatsComponent> _statsStash;

        [Inject] private StatsPanel _statsPanel;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<StatsComponent>().With<AliveComponent>().Without<AIComponent>().Build();
            _statsStash = World.GetStash<StatsComponent>();
            
            foreach (var entity in _filter)
            {
                ref var statsComponent = ref _statsStash.Get(entity);
                _statsPanel.Initiate(statsComponent.Value.Stats);
            }
        }
    }
}