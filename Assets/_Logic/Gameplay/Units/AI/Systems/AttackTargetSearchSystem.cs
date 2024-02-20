using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Buildings.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class AttackTargetSearchSystem : QuerySystem
    {
        private readonly Collider[] _colliders = new Collider[10];
        private readonly int _layer = LayerMask.GetMask("Unit");
        
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<TeamIdComponent>().With<TransformComponent>()
                .Without<AttackTargetComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent, ref TeamIdComponent teamIdComponent, ref TransformComponent transformComponent) =>
                {
                    var aiSettings = ConfigsManager.GetConfig<AISettings>();
                    var position = transformComponent.Value.position;
                    var searchRange  = attackComponent.CurrentData.Range * aiSettings.TargetSearchRangeToAttackRangeRatio;
                    var collisions = Physics.OverlapSphereNonAlloc(position, searchRange, _colliders, _layer);
                    var minDistance = float.MaxValue;
                    Entity targetEntity = null;

                    for (int i = 0; i < collisions; i++)
                    {
                        if (_colliders[i].TryGetComponent(out UnitProvider targetProvider) &&
                            !targetProvider.Entity.IsNullOrDisposed() &&
                            targetProvider.Entity.GetComponent<TeamIdComponent>().Value != teamIdComponent.Value)
                        {
                            if (EcsExtensions.TryGetDistanceBetweenClosestPointsOfEntitiesColliders(entity, targetProvider.Entity, out var distance) && 
                                distance < attackComponent.CurrentData.Range && distance < minDistance)
                            {
                                minDistance = distance;
                                targetEntity = targetProvider.Entity;
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