using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class EnemySearchingSystem : QuerySystem
    {
        private readonly Collider[] _colliders = new Collider[10];
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<TeamIdComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent, ref TeamIdComponent teamIdComponent, ref TransformComponent transformComponent) =>
                {
                    var aiSettings = ConfigsManager.GetConfig<AISettings>();
                    var searchingRange = attackComponent.CurrentData.Range * aiSettings.TargetSearchingRangeToAttackRangeRatio;
                    
                    if (entity.Has<AttackTargetComponent>())
                    {
                        var targetEntity = entity.GetComponent<AttackTargetComponent>().TargetEntity;

                        if (targetEntity.IsNullOrDisposed() || 
                            !targetEntity.TryGetComponentValue<TransformComponent>(out var targetTransformComponent) || 
                            (targetTransformComponent.Value.position - transformComponent.Value.position).magnitude > searchingRange)
                        {
                            entity.RemoveComponent<AttackTargetComponent>();

                            if (entity.Has<DestinationComponent>())
                            {
                                entity.RemoveComponent<DestinationComponent>();
                            }
                        }
                    }
                    else
                    {
                        var collisions = Physics.OverlapSphereNonAlloc(transformComponent.Value.position, searchingRange, _colliders);

                        for (int i = 0; i < collisions; i++)
                        {
                            if (_colliders[i].TryGetComponent(out UnitProvider enemyProvider) &&
                                !enemyProvider.Entity.IsNullOrDisposed() &&
                                enemyProvider.Entity.TryGetComponentValue<TeamIdComponent>(out var enemyTeamIdComponent) &&
                                enemyTeamIdComponent.Value != teamIdComponent.Value)
                            {
                                entity.SetComponent(new AttackTargetComponent
                                {
                                    TargetEntity = enemyProvider.Entity
                                });
                                
                                break;
                            }
                        }
                    }
                });
        }
    }
}