using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Movement;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class EnemyFollowingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<TransformComponent>().With<AIComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent, ref AttackTargetComponent targetComponent, ref TransformComponent transformComponent) =>
                {
                    if (targetComponent.TargetEntity.IsNullOrDisposed() || 
                        !targetComponent.TargetEntity.TryGetComponentValue<TransformComponent>(out var targetTransformComponent)) return;

                    var direction = transformComponent.Value.position - targetTransformComponent.Value.position;
                    var distance = direction.magnitude;
                    var targetIsClose = distance <= attackComponent.CurrentData.Range;
                    var destination = targetIsClose
                        ? transformComponent.Value.position
                        : targetTransformComponent.Value.position + direction.normalized * attackComponent.CurrentData.Range;
                    
                    World.GetRequest<DestinationChangeRequest>().Publish(new DestinationChangeRequest
                    {
                        Entity = entity,
                        Destination = destination
                    });
                });
        }
    }
}