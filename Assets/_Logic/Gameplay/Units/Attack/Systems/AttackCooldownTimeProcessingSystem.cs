using _Logic.Core;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class AttackCooldownTimeProcessingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<AttackComponent> _attackStash;
        private Stash<StatsComponent> _statsStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<AttackComponent>().With<StatsComponent>().With<AliveComponent>().Build();
            _attackStash = World.GetStash<AttackComponent>();
            _statsStash = World.GetStash<StatsComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var attackComponent = ref _attackStash.Get(entity);
                ref var statsComponent = ref _statsStash.Get(entity);

                var attackSpeed = statsComponent.Value.GetCurrentValue(StatType.AttackSpeed);
                var attackTime = statsComponent.Value.GetCurrentValue(StatType.AttackTime);
                var actualAttackTime = attackTime / (attackSpeed * 0.01f);
                
                if (attackSpeed <= 0.01f)
                    actualAttackTime = float.MaxValue;
                
                attackComponent.AttacksPerSecond = 1 / actualAttackTime;
                attackComponent.AttackTime = actualAttackTime;
                attackComponent.RemainingAttackTime = actualAttackTime * (1 - attackComponent.AttackTimePercentage);

                if (attackComponent.AttackTimePercentage < 1)
                {
                    attackComponent.RemainingAttackTime -= deltaTime;
                    
                    if (attackComponent.RemainingAttackTime < 0)
                        attackComponent.RemainingAttackTime = 0;
                }

                attackComponent.AttackTimePercentage = Mathf.Clamp01(1 - attackComponent.RemainingAttackTime / attackComponent.AttackTime);
            }
        }
    }
}