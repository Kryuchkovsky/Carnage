using _Logic.Core.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Movement;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class EnemyFollowingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<MovementComponent>().With<AttackComponent>().With<AttackTargetComponent>()
                .With<TransformComponent>().With<AIComponent>()
                .ForEach((Entity entity, ref MovementComponent movementComponent, ref AttackComponent attackComponent, 
                    ref AttackTargetComponent targetComponent, ref TransformComponent transformComponent) =>
                {
                    if (!movementComponent.CanMove || 
                        targetComponent.TargetEntity.IsNullOrDisposed() || 
                        !targetComponent.TargetEntity.TryGetComponentValue<TransformComponent>(out var targetTransformComponent) || 
                        (entity.TryGetComponentValue<DestinationComponent>(out var destinationComponent) && 
                         destinationComponent.Value == targetTransformComponent.Value.position)) return;
                    
                    World.GetRequest<DestinationChangeRequest>().Publish(new DestinationChangeRequest
                    {
                        Entity = entity,
                        Destination = targetTransformComponent.Value.position
                    });
                });
        }
    }
}