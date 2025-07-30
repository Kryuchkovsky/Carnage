using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.HealthBar;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.Units.Health.Systems
{
    public sealed class HealthBarProvidingSystem : AbstractUpdateSystem
    {
        private Filter _filter1;
        private Filter _filter2;
        private Stash<UnitComponent> _unitStash;
        private Stash<TeamComponent> _teamStash;
        private Stash<BoundsComponent> _boundsStash;
        private Stash<HealthBarComponent> _healthBarStash;

        [Inject] private HealthBarsService _healthBarsService;
        
        private readonly float _additionalOffsetY = 1;

        public override void OnAwake()
        {
            _filter1 = World.Filter.With<UnitComponent>().With<BoundsComponent>().With<HealthComponent>()
                .With<TeamComponent>().With<AliveComponent>().Without<HealthBarComponent>().Build();
            _filter2 = World.Filter.With<UnitComponent>().With<BoundsComponent>().With<HealthComponent>()
                .With<TeamComponent>().With<HealthBarComponent>().Without<AliveComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _boundsStash = World.GetStash<BoundsComponent>();
            _healthBarStash = World.GetStash<HealthBarComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter1)
            {
                ref var unitComponent = ref _unitStash.Get(entity);
                ref var teamDataComponent = ref _teamStash.Get(entity, out var hasTeamComponent);
                ref var boundsComponent = ref _boundsStash.Get(entity);
                var isAlly = hasTeamComponent && teamDataComponent.Id == 0;
                var offsetY = boundsComponent.Value.max.y + _additionalOffsetY;
                var healthBar = _healthBarsService.CreateHealthBar(unitComponent.Value.transform, offsetY, isAlly);
                
                _healthBarStash.Set(entity, new HealthBarComponent
                {
                    Value = healthBar
                });
            }

            foreach (var entity in _filter2)
            {
                ref var healthBarComponent = ref _healthBarStash.Get(entity);
                _healthBarsService.RemoveHealthBar(healthBarComponent.Value);
            }
        }
    }
}