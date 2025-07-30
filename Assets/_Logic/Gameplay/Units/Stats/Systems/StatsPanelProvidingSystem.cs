using _Logic.Core;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Stats.Systems
{
    public sealed class StatsPanelProvidingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<StatsPanelComponent> _statsPanelStash;
        private Stash<StatsComponent> _statsStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<StatsComponent>().With<AliveComponent>()
                .Without<StatsPanelComponent>().Without<AIComponent>().Build();
            _statsPanelStash = World.GetStash<StatsPanelComponent>();
            _statsStash = World.GetStash<StatsComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var statsComponent = ref _statsStash.Get(entity);
                
                _statsPanelStash.Set(entity, new StatsPanelComponent
                {
                    Value = StatsPanel.Instance
                });

                StatsPanel.Instance.Initiate(statsComponent.Value.Stats);
            }
        }
    }
}