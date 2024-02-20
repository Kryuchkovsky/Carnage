using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Creatures.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class UnitSightHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<CreatureComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref AttackTargetComponent targetComponent, ref TransformComponent transformComponent) =>
                {
                    if (targetComponent.TargetEntity.IsNullOrDisposed() || 
                        !targetComponent.TargetEntity.Has<TransformComponent>() || 
                        !targetComponent.IsInAttackRadius) return;

                    var targetTransform = targetComponent.TargetEntity.GetComponent<TransformComponent>().Value;
                    var direction = (transformComponent.Value.position - targetTransform.position).normalized;
                    transformComponent.Value.rotation = Quaternion.LookRotation(-direction);
                });
        }
    }
}