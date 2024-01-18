using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class TargetFollowingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<TargetComponent>().With<DestinationComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent, ref TargetComponent targetComponent, 
                    ref DestinationComponent destinationComponent, ref TransformComponent transformComponent) =>
                {
                    if (targetComponent.TargetEntity.IsNullOrDisposed() || 
                        !targetComponent.TargetEntity.TryGetComponentValue<TransformComponent>(out var targetTransformComponent)) return;

                    var direction = transformComponent.Value.position - targetTransformComponent.Value.position;
                    var distance = direction.magnitude;
                    var targetIsClose = distance <= attackComponent.CurrentData.Range;

                    if (targetIsClose)
                    {
                        destinationComponent.Value = transformComponent.Value.position;
                        transformComponent.Value.rotation = Quaternion.LookRotation(-direction);
                    }
                    else
                    {
                        var targetPosition = targetTransformComponent.Value.position + 
                                             direction.normalized * attackComponent.CurrentData.Range;
                        destinationComponent.Value = targetPosition;
                    }
                });
        }
    }
}