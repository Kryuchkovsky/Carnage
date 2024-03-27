using _Logic.Core.Components;
using _Logic.Gameplay.Input.Components;
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
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref TransformComponent transformComponent, 
                    ref MovementComponent movementComponent, ref StatsComponent statsComponent, ref DestinationComponent destinationData) =>
                {
                    var inputDataComponent = World.Filter.With<InputDataComponent>().Build().First().GetComponent<InputDataComponent>();
                    var movementDirection = new Vector3(inputDataComponent.Direction.x, 0, inputDataComponent.Direction.y);
                    var destination = transformComponent.Value.position + movementDirection;
                    unitComponent.Value.OnMove(inputDataComponent.Direction.magnitude);
                    entity.SetComponent(new DestinationComponent
                    {
                        Value = destination
                    });

                    if (destinationData.Value != transformComponent.Value.position && statsComponent.Value.TryGetCurrentValue(StatType.MovementSpeed, out var movementSpeed))
                    {
                        var direction = (destinationData.Value - transformComponent.Value.position).normalized;
                        var step = direction * movementSpeed * deltaTime;
                        transformComponent.Value.Translate(step, Space.World);
                        transformComponent.Value.rotation = Quaternion.LookRotation(direction);
                    }
                });
        }
    }
}