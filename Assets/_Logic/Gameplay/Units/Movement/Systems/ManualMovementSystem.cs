using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class ManualMovementSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<TransformComponent>().With<MovementComponent>().With<StatsComponent>().With<DestinationComponent>().With<AliveComponent>()
                .Without<AIComponent>()
                .ForEach((Entity entity, ref TransformComponent transformComponent, ref StatsComponent statsComponent, ref DestinationComponent destinationComponent) =>
                {
                    if (destinationComponent.Value == transformComponent.Value.position) return;

                    var movementSpeed = statsComponent.Value.GetCurrentValue(StatType.MovementSpeed);
                    var direction = (destinationComponent.Value - transformComponent.Value.position).normalized;
                    var step = direction * movementSpeed * deltaTime;
                    transformComponent.Value.Translate(step, Space.World);
                    transformComponent.Value.rotation = Quaternion.LookRotation(direction);
                });
        }
    }
}