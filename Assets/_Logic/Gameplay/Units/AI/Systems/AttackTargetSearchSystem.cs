using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class AttackTargetSearchSystem : QuerySystem
    {
        private readonly Collider[] _colliders = new Collider[10];

        [Inject] private AISettings _aiSettings;
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<StatsComponent>().With<AttackComponent>().With<TeamComponent>().With<TransformComponent>().With<AliveComponent>()
                .Without<AttackTargetComponent>()
                .ForEach((Entity entity, ref StatsComponent statsComponent, ref AttackComponent attackComponent, ref TeamComponent teamDataComponent, ref TransformComponent transformComponent) =>
                {
                    var position = transformComponent.Value.position;
                    var range = statsComponent.Value.GetCurrentValue(StatType.AttackRange);
                    var searchRange  = range * _aiSettings.TargetSearchRangeToAttackRangeRatio;
                    var collisions = Physics.OverlapSphereNonAlloc(position, searchRange, _colliders, 1 << teamDataComponent.EnemiesLayer);
                    var minDistance = float.MaxValue;
                    Entity targetEntity = null;
                    
                    for (int i = 0; i < collisions; i++)
                    {
                        if (_colliders[i].TryGetComponent(out LinkedCollider collider) &&
                            !collider.Entity.IsNullOrDisposed() &&
                            collider.Entity.GetComponent<TeamComponent>().Id != teamDataComponent.Id)
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