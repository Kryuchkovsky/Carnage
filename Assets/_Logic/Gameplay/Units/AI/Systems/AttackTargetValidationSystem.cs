using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class AttackTargetValidationSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<AttackTargetComponent> _attackTargetStash;
        private Stash<StatsComponent> _statsStash;
        private Stash<AliveComponent> _aliveStash;
        private Stash<AIComponent> _aiStash;
        private Stash<PriorityComponent> _priorityStash;

        [Inject] private AISettings _aiSettings;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>()
                .With<StatsComponent>().With<AliveComponent>().Build();
            _attackTargetStash = World.GetStash<AttackTargetComponent>();
            _statsStash = World.GetStash<StatsComponent>();
            _aliveStash = World.GetStash<AliveComponent>();
            _aiStash = World.GetStash<AIComponent>();
            _priorityStash = World.GetStash<PriorityComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var statsComponent = ref _statsStash.Get(entity);
                ref var attackTargetComponent = ref _attackTargetStash.Get(entity);
                
                var range = statsComponent.Value.GetCurrentValue(StatType.AttackRange);
                var followingRange = range * _aiSettings.TargetSearchRangeToAttackRangeRatio;
                var distanceIsGotten = EcsExtensions.TryGetDistanceBetweenClosestPoints(
                    entity, attackTargetComponent.TargetEntity, out var distance);
                var isAI = _aiStash.Has(entity);
                var hasPrioritizedTarget = _priorityStash.Has(attackTargetComponent.TargetEntity);
                var targetIsAlive = _aliveStash.Has(attackTargetComponent.TargetEntity);

                var targetIsCorrected = targetIsAlive && distanceIsGotten && 
                                        ((isAI && (distance < followingRange || hasPrioritizedTarget)) || 
                                         (!isAI && distance < range));
                
                if (targetIsCorrected)
                    attackTargetComponent.IsInAttackRadius = distance < range;
                else _attackTargetStash.Remove(entity);
            }
        }
    }
}