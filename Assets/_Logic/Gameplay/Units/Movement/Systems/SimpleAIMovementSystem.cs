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

namespace _Logic.Gameplay.Units.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class SimpleAIMovementSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitComponent> _unitStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<StatsComponent> _statsStash;
        private Stash<DestinationComponent> _destinationStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<DestinationComponent>().With<MovementComponent>()
                .With<StatsComponent>().With<AIComponent>().With<AliveComponent>().Without<PathfinderComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _statsStash = World.GetStash<StatsComponent>();
            _destinationStash = World.GetStash<DestinationComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var unitComponent = ref _unitStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                ref var destinationComponent = ref _destinationStash.Get(entity);
                
                var transform = transformComponent.Value;
                var direction = (destinationComponent.Value - transform.position);
                direction.y = 0;
                var distance = direction.magnitude + 0.1f;
                var normalizedSpeed = 0;

                if (distance <= 0.1f)
                {
                    _destinationStash.Remove(entity);
                }
                else
                {
                    ref var statsComponent = ref _statsStash.Get(entity);
                    var speed = statsComponent.Value.GetCurrentValue(StatType.MovementSpeed);
                    transform.position += direction.normalized * speed * deltaTime;
                    normalizedSpeed = 1;
                }

                unitComponent.Value.OnMove(normalizedSpeed);
            }
        }
    }
}