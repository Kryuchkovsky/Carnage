using System;
using _Logic.Core;
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
    public sealed class PlayerUnitDestinationSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitComponent> _unitStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<DestinationComponent> _destinationStash;

        [Inject] private InputService _inputService;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<MovementComponent>()
                .With<AliveComponent>().Without<AIComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var unitComponent = ref _unitStash.Get(entity);
                unitComponent.Value.OnMove(_inputService.Direction.magnitude);
                var movementDirection = new Vector3(_inputService.Direction.x, 0, _inputService.Direction.y);
                var destination = transformComponent.Value.position + movementDirection;
                _destinationStash.Set(entity, new DestinationComponent
                {
                    Value = destination
                });
            }
        }
    }
}