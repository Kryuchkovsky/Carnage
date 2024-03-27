using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class UnitSightHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<TransformComponent>().With<AliveComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref TransformComponent transformComponent) =>
                {
                    Vector3 sightPosition;
                    ref var targetComponent = ref entity.GetComponent<AttackTargetComponent>(out var hasTargetComponent);
                    
                    if (hasTargetComponent && !targetComponent.TargetEntity.IsNullOrDisposed() &&
                        targetComponent.TargetEntity.Has<TransformComponent>() && targetComponent.IsInAttackRadius)
                    {
                        sightPosition = targetComponent.TargetEntity.GetComponent<TransformComponent>().Value.position;
                    }
                    else
                    {
                        sightPosition = transformComponent.Value.position + transformComponent.Value.forward;
                    }
                    
                    unitComponent.Value.Model.LookAtPoint(sightPosition);
                });
        }
    }
}