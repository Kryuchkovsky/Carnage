using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
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
    public sealed class AttackTargetValidationSystem : QuerySystem
    {
        [Inject] private AISettings _aiSettings;

        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<StatsComponent>().With<AliveComponent>()
                .ForEach((Entity entity, ref AttackTargetComponent attackTargetComponent, ref StatsComponent statsComponent) =>
                {
                    var range = statsComponent.Value.GetCurrentValue(StatType.AttackRange);
                    var followingRange = range * _aiSettings.TargetSearchRangeToAttackRangeRatio;
                    var distanceIsGotten = EcsExtensions.TryGetDistanceBetweenClosestPointsOfEntitiesColliders(
                        entity, attackTargetComponent.TargetEntity, out var distance);
                    
                    if (attackTargetComponent.TargetEntity.Has<AliveComponent>() && distanceIsGotten && 
                        ((entity.Has<AIComponent>() && (distance < followingRange || attackTargetComponent.TargetEntity.Has<PriorityComponent>())) || 
                         (!entity.Has<AIComponent>() && distance < range)))
                    {
                        attackTargetComponent.IsInAttackRadius = distance < range;
                    }
                    else
                    {
                        entity.RemoveComponent<AttackTargetComponent>();

                        if (entity.Has<DestinationComponent>())
                        {
                            entity.RemoveComponent<DestinationComponent>();
                        }
                    }
                });
        }
    }
}