using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class AttackTargetSearchSystem : QuerySystem
    {
        private readonly Collider[] _colliders = new Collider[10];
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<StatsComponent>().With<AttackComponent>().With<TeamDataComponent>().With<TransformComponent>().With<AliveComponent>()
                .Without<AttackTargetComponent>()
                .ForEach((Entity entity, ref StatsComponent statsComponent, ref AttackComponent attackComponent, ref TeamDataComponent teamDataComponent, ref TransformComponent transformComponent) =>
                {
                    var aiSettings = ConfigManager.Instance.GetConfig<AISettings>();
                    var position = transformComponent.Value.position;
                    statsComponent.Value.TryGetCurrentValue(StatType.AttackRange, out var range);
                    var searchRange  = range * aiSettings.TargetSearchRangeToAttackRangeRatio;
                    var collisions = Physics.OverlapSphereNonAlloc(position, searchRange, _colliders, 1 << teamDataComponent.EnemiesLayer);
                    var minDistance = float.MaxValue;
                    Entity targetEntity = null;
                    
                    for (int i = 0; i < collisions; i++)
                    {
                        if (_colliders[i].TryGetComponent(out LinkedCollider collider) &&
                            !collider.Entity.IsNullOrDisposed() &&
                            collider.Entity.GetComponent<TeamDataComponent>().Id != teamDataComponent.Id)
                        {
                            if (EcsExtensions.TryGetDistanceBetweenClosestPointsOfEntitiesColliders(entity, collider.Entity, out var distance) && 
                                distance < searchRange && distance < minDistance)
                            {
                                minDistance = distance;
                                targetEntity = collider.Entity;
                            }
                        }
                    }

                    if (!targetEntity.IsNullOrDisposed())
                    {
                        entity.SetComponent(new AttackTargetComponent
                        {
                            TargetEntity = targetEntity
                        });
                    }
                });
        }
    }
}