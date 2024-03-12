using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

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
                    var aiSettings = ConfigManager.GetConfig<AISettings>();
                    var followingRange = attackComponent.Stats.Range.CurrentValue * aiSettings.TargetSearchRangeToAttackRangeRatio;
                    var distanceIsGotten = EcsExtensions.TryGetDistanceBetweenClosestPointsOfEntitiesColliders(
                        entity, attackTargetComponent.TargetEntity, out var distance);
                    
                    if (distanceIsGotten && 
                        ((entity.Has<AIComponent>() && distance < followingRange) || 
                        (!entity.Has<AIComponent>() && distance < attackComponent.Stats.Range.CurrentValue)))
                    {
                        attackTargetComponent.IsInAttackRadius = distance < attackComponent.Stats.Range.CurrentValue;
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