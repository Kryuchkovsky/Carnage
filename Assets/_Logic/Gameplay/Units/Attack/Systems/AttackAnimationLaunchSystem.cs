using _Logic.Core;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class AttackAnimationLaunchSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitComponent> _unitStash;
        private Stash<AttackComponent> _attackStash;
        private Stash<AttackTargetComponent> _attackTargetStash;
        private Event<AttackAnimationCompletionEvent> _event;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<AttackComponent>().With<AttackTargetComponent>()
                .With<AliveComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _attackTargetStash = World.GetStash<AttackTargetComponent>();
            _event = World.GetEvent<AttackAnimationCompletionEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var unitComponent = ref _unitStash.Get(entity);
                ref var attackComponent = ref _attackStash.Get(entity);
                ref var attackTargetComponent = ref _attackTargetStash.Get(entity);
                
                if (!attackTargetComponent.IsInAttackRadius || attackComponent.AttackTimePercentage < 1) 
                    continue;

                var receiverEntity = attackTargetComponent.TargetEntity;
                    
                attackComponent.AttackTimePercentage = 0;
                unitComponent.Value.OnAttack(attackComponent.AttacksPerSecond, () =>
                {
                    if (World.IsDisposed(entity) || World.IsDisposed(receiverEntity)) 
                        return;
                        
                    _event.NextFrame(new AttackAnimationCompletionEvent
                    {
                        Entity = entity
                    });
                });
            }
        }
    }
}