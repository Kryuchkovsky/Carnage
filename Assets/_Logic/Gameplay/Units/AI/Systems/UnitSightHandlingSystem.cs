using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class UnitSightHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent,  ref AttackTargetComponent targetComponent, ref TransformComponent transformComponent) =>
                {
                    if (targetComponent.TargetEntity.IsNullOrDisposed() || 
                        !targetComponent.TargetEntity.Has<TransformComponent>() || 
                        !targetComponent.IsInAttackRadius) return;

                    var sightPosition = targetComponent.TargetEntity.GetComponent<TransformComponent>().Value.position;
                    unitComponent.Value.Model.LookAtPoint(sightPosition);
                });
        }
    }
}