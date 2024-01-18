using _Logic.Gameplay.Units.Attack.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class AttackCooldownTimeProcessingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<AttackComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent) =>
                {
                    attackComponent.AttacksPerSecond = attackComponent.CurrentData.Speed * 0.01f / 
                                                       attackComponent.CurrentData.BasicAttackTime;

                    attackComponent.AttackTime = 1 / attackComponent.AttacksPerSecond;
                    
                    attackComponent.RemainingAttackTime = Mathf.Lerp(0, attackComponent.AttackTime,
                        1 - attackComponent.AttackTimePercentage);

                    if (attackComponent.AttackTimePercentage < 1)
                    {
                        attackComponent.RemainingAttackTime -= deltaTime;
                    }

                    attackComponent.AttackTimePercentage = 1 - Mathf.InverseLerp(0, attackComponent.AttackTime, 
                        attackComponent.RemainingAttackTime);
                });
        }
    }
}