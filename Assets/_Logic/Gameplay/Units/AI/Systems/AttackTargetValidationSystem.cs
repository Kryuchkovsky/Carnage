using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class AttackTargetValidationSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent, ref AttackTargetComponent attackTargetComponent) =>
                {
                    var aiSettings = ConfigsManager.GetConfig<AISettings>();
                    var followingRange = attackComponent.CurrentData.Range * aiSettings.TargetSearchRangeToAttackRangeRatio;
                    var distanceIsGotten = EcsExtensions.TryGetDistanceBetweenClosestPointsOfEntitiesColliders(
                        entity, attackTargetComponent.TargetEntity, out var distance);

                    if (distanceIsGotten && 
                        ((entity.Has<AIComponent>() && distance < followingRange) || 
                        (!entity.Has<AIComponent>() && distance < attackComponent.CurrentData.Range)))
                    {
                        attackTargetComponent.IsInAttackRadius = distance < attackComponent.CurrentData.Range;
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