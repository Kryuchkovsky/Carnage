using _Logic.Core.Components;
using _Logic.Gameplay.Projectiles.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Projectiles.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileDestinationUpdateSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<TargetComponent>().With<FlightParametersComponent>()
                .ForEach((Entity entity, ref TargetComponent targetComponent) =>
                {
                    ref var targetTransformComponent = ref targetComponent.Entity.GetComponent<TransformComponent>(out var targetHasTransformComponent);
                        
                    if (targetHasTransformComponent)
                    {
                        var targetPosition = targetTransformComponent.Value.position;
                        ref var targetBoundsComponent = ref targetComponent.Entity.GetComponent<BoundsComponent>(out var targetHasBoundsComponent);

                        if (targetHasBoundsComponent)
                        {
                            targetPosition += Vector3.up * targetBoundsComponent.Value.extents.y;
                        }
                        
                        entity.SetComponent(new DestinationComponent
                        {
                            Value = targetPosition
                        });
                    }
                });
        }
    }
}