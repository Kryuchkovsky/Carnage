using _Logic.Core.Components;
using _Logic.Extensions.HealthBar;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.Units.Health.Systems
{
    public sealed class HealthBarProvidingSystem : QuerySystem
    {
        [Inject] private HealthBarsService _healthBarsService;
        
        private readonly float _additionalOffsetY = 1;

        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<BoundsComponent>().With<HealthComponent>().With<TeamDataComponent>().With<AliveComponent>()
                .Without<HealthBarComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref BoundsComponent boundsComponent) =>
                {
                    var teamDataComponent = entity.GetComponent<TeamDataComponent>(out var hasTeamComponent);
                    var isAlly = hasTeamComponent && teamDataComponent.Id == 0;
                    var offsetY = boundsComponent.Value.max.y + _additionalOffsetY;
                    var healthBar = _healthBarsService.CreateHealthBar(unitComponent.Value.transform, offsetY, isAlly);
                    entity.SetComponent(new HealthBarComponent
                    {
                        Value = healthBar
                    });
                });
            
            CreateQuery()
                .With<UnitComponent>().With<BoundsComponent>().With<HealthComponent>().With<TeamDataComponent>().With<HealthBarComponent>()
                .Without<AliveComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref HealthBarComponent healthBarComponent) =>
                {
                    _healthBarsService.RemoveHealthBar(healthBarComponent.Value);
                });
        }
    }
}