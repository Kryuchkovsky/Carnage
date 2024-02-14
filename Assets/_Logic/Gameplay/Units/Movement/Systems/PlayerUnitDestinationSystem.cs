using _Logic.Core.Components;
using _Logic.Gameplay.Input.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUnitDestinationSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<TransformComponent>().With<MovementComponent>()
                .Without<AIComponent>()
                .ForEach((Entity entity, ref TransformComponent transformComponent) =>
                {
                    var inputDataComponent = World.Filter.With<InputDataComponent>().Build().First().GetComponent<InputDataComponent>();
                    var movementDirection = new Vector3(inputDataComponent.Direction.x, 0, inputDataComponent.Direction.y);
                    var destination = transformComponent.Value.position + movementDirection;
                    entity.SetComponent(new DestinationComponent
                    {
                        EndValue = destination
                    });
                });
        }
    }
}