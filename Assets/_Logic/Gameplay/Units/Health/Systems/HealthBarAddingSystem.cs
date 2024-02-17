using _Logic.Core.Components;
using _Logic.Extensions.HealthBar;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Health.Systems
{
    public sealed class HealthBarAddingSystem : QuerySystem
    {
        private readonly float _additionalOffsetY = 1;
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<BoundsComponent>().With<HealthComponent>()
                .Without<HealthBarComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref BoundsComponent boundsComponent) =>
                {
                    var isAlly = entity.TryGetComponentValue<TeamIdComponent>(out var teamIdComponent) && teamIdComponent.Value == 0;
                    var offsetY = boundsComponent.Value.max.y + _additionalOffsetY;
                    var healthBar = HealthBarsService.Instance.CreateHealthBar(unitComponent.Value.transform, offsetY, isAlly);
                    entity.SetComponent(new HealthBarComponent
                    {
                        Value = healthBar
                    });
                });
        }
    }
}