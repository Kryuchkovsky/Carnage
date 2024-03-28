using _Logic.Core.Components;
using _Logic.Extensions.Input;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerUnitDestinationSystem : QuerySystem
    {
        [Inject]
        private InputService _inputService;
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<TransformComponent>().With<MovementComponent>().With<AliveComponent>()
                .Without<AIComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref TransformComponent transformComponent) =>
                {
                    unitComponent.Value.OnMove(_inputService.Direction.magnitude);
                    var movementDirection = new Vector3(_inputService.Direction.x, 0, _inputService.Direction.y);
                    var destination = transformComponent.Value.position + movementDirection;
                    entity.SetComponent(new DestinationComponent
                    {
                        Value = destination
                    });
                });
        }
    }
}