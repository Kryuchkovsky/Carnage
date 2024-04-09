﻿using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class AttackAnimationLaunchSystem : QuerySystem
    {
        private Event<AttackAnimationCompletionEvent> _event;
        
        public override void OnAwake()
        {
            base.OnAwake();
            _event = World.GetEvent<AttackAnimationCompletionEvent>();
        }

        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>().With<AliveComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref AttackComponent attackComponent, 
                    ref AttackTargetComponent attackTargetComponent) =>
                {
                    if (!attackTargetComponent.IsInAttackRadius || attackComponent.AttackTimePercentage < 1) return;

                    var receiverEntity = attackTargetComponent.TargetEntity;
                    
                    attackComponent.AttackTimePercentage = 0;
                    unitComponent.Value.OnAttack(attackComponent.AttacksPerSecond, () =>
                    {
                        if (entity.IsNullOrDisposed() && receiverEntity.IsNullOrDisposed()) return;
                        
                        _event.NextFrame(new AttackAnimationCompletionEvent
                        {
                            Entity = entity
                        });
                    });
                });
        }
    }
}