using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class MeleeAttackHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<MeleeAttackComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref AttackComponent attackComponent, 
                    ref AttackTargetComponent attackTargetComponent, ref TransformComponent transformComponent) =>
                {
                    if (!attackTargetComponent.IsInAttackRadius || attackComponent.AttackTimePercentage < 1) return;
                    
                    World.GetRequest<DamageRequest>().Publish(new DamageRequest
                    {
                        AttackerEntity = entity,
                        ReceiverEntity = attackTargetComponent.TargetEntity,
                        Damage = attackComponent.CurrentData.Damage
                    });

                    // if (attackTargetComponent.TargetEntity.TryGetComponentValue<RigidbodyComponent>(out var enemyRigidbodyComponent))
                    // {
                    //     var force = direction.normalized * 2;
                    //     enemyRigidbodyComponent.Value.AddForce(force, ForceMode.VelocityChange);
                    // }

                    attackComponent.AttackTimePercentage = 0;
                    unitComponent.Value.OnAttack();
                });
        }
    }
}