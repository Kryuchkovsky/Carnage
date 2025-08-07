using _Logic.Core;
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
    public class ManualMovementSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<TransformComponent> _transformStash;
        private Stash<StatsComponent> _statsStash;
        private Stash<DestinationComponent> _destinationStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<MovementComponent>()
                .With<StatsComponent>().With<DestinationComponent>().With<AliveComponent>()
                .Without<AIComponent>().Build();
            _transformStash = World.GetStash<TransformComponent>();
            _statsStash = World.GetStash<StatsComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var destinationComponent = ref _destinationStash.Get(entity);
                
                if (destinationComponent.Value == transformComponent.Value.position) 
                    continue;
                
                ref var statsComponent = ref _statsStash.Get(entity);

                var movementSpeed = statsComponent.Value.GetCurrentValue(StatType.MovementSpeed);
                var direction = (destinationComponent.Value - transformComponent.Value.position).normalized;
                var step = direction * movementSpeed * deltaTime;
                transformComponent.Value.Translate(step, Space.World);
                transformComponent.Value.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}