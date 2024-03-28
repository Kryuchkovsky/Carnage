using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class AttackCooldownTimeProcessingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<AttackComponent>().With<StatsComponent>().With<AliveComponent>()
                .ForEach((Entity entity, ref AttackComponent attackComponent, ref StatsComponent statsComponent) =>
                {
                    var attackSpeed = statsComponent.Value.GetCurrentValue(StatType.AttackSpeed);
                    var attackTime = statsComponent.Value.GetCurrentValue(StatType.AttackTime);
                    attackComponent.AttacksPerSecond = attackSpeed * 0.01f / attackTime;
                    attackComponent.AttackTime = 1 / attackComponent.AttacksPerSecond;
                    attackComponent.RemainingAttackTime = Mathf.Lerp(0, attackComponent.AttackTime, 1 - attackComponent.AttackTimePercentage);

                    if (attackComponent.AttackTimePercentage < 1)
                    {
                        attackComponent.RemainingAttackTime -= deltaTime;
                    }

                    attackComponent.AttackTimePercentage = 1 - Mathf.InverseLerp(0, attackComponent.AttackTime, attackComponent.RemainingAttackTime);
                });
        }
    }
}