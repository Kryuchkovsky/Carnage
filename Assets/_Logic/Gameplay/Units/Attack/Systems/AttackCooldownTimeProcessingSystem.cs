using System;
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
                attackComponent.AttacksPerSecond = attackSpeed * 0.01f / attackTime;
                attackComponent.AttackTime = 1 / attackComponent.AttacksPerSecond;
                attackComponent.RemainingAttackTime = Mathf.Lerp(0, attackComponent.AttackTime, 1 - attackComponent.AttackTimePercentage);

                if (attackComponent.AttackTimePercentage < 1)
                {
                    attackComponent.RemainingAttackTime -= deltaTime;
                }

                attackComponent.AttackTimePercentage = 1 - Mathf.InverseLerp(0, attackComponent.AttackTime, attackComponent.RemainingAttackTime);
            }
        }
    }
}