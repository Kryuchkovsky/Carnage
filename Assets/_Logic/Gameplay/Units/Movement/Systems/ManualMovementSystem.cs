using _Logic.Core.Components;
using _Logic.Gameplay.Input.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Movement.Systems
{
    public class ManualMovementSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<TransformComponent>().With<MovementComponent>().With<DestinationComponent>()
                .Without<AIComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref TransformComponent transformComponent, 
                    ref MovementComponent movementComponent, ref DestinationComponent destinationData) =>
                {
                    var inputDataComponent = World.Filter.With<InputDataComponent>().Build().First().GetComponent<InputDataComponent>();
                    var movementDirection = new Vector3(inputDataComponent.Direction.x, 0, inputDataComponent.Direction.y);
                    var destination = transformComponent.Value.position + movementDirection;
                    entity.SetComponent(new DestinationComponent
                    {
                        Value = destination
                    });
                    
                    unitComponent.Value.OnMove(inputDataComponent.Direction.magnitude);
                    
                    if (destinationData.Value == transformComponent.Value.position) return;
                    
                    var direction = (destinationData.Value - transformComponent.Value.position).normalized;
                    var step = direction * movementComponent.CurrentData.MovementSpeed * deltaTime;
                    transformComponent.Value.Translate(step, Space.World);
                    transformComponent.Value.rotation = Quaternion.LookRotation(direction);
                });
        }
    }
}