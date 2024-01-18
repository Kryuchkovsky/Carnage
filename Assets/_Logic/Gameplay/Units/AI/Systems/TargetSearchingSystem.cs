using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class TargetSearchingSystem : QuerySystem
    {
        private readonly Collider[] _colliders = new Collider[10];
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<TeamIdComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent, ref TeamIdComponent teamIdComponent,
                    ref TransformComponent transformComponent) =>
                {
                    var aiSettings = ConfigsManager.GetConfig<AISettings>();
                    var searchingRange = attackComponent.CurrentData.Range * aiSettings.TargetSearchingRangeToAttackRangeRatio;
                    
                    if (entity.Has<TargetComponent>())
                    {
                        var targetEntity = entity.GetComponent<TargetComponent>().TargetEntity;

                        if (targetEntity.IsNullOrDisposed() || !targetEntity.TryGetComponentValue<TransformComponent>(out var targetTransformComponent))
                        {
                            entity.RemoveComponent<TargetComponent>();
                        }
                        else
                        {
                            var targetIsFar = (targetTransformComponent.Value.position -
                                               transformComponent.Value.position).magnitude > searchingRange;

                            if (targetIsFar)
                            {
                                entity.RemoveComponent<TargetComponent>();
                            }
                        }
                    }
                    
                    var collisions = Physics.OverlapSphereNonAlloc(
                        transformComponent.Value.position, searchingRange, _colliders);

                    for (int i = 0; i < collisions; i++)
                    {
                        if (_colliders[i].TryGetComponent(out UnitProvider enemyProvider) &&
                            !enemyProvider.Entity.IsNullOrDisposed() &&
                            enemyProvider.Entity.TryGetComponentValue<TeamIdComponent>(out var enemyTeamIdComponent) &&
                            enemyTeamIdComponent.Value != teamIdComponent.Value)
                        {
                            entity.SetComponent(new TargetComponent
                            {
                                TargetEntity = enemyProvider.Entity
                            });
                        }
                    }
                });
        }
    }
}