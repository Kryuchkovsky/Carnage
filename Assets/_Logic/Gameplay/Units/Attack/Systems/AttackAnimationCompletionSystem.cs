using _Logic.Core;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Attack.Requests;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackAnimationCompletionSystem : AbstractUpdateSystem
    {
        private Event<AttackAnimationCompletionEvent> _attackAnimationCompletionEvent;
        private Event<AttackStartEvent> _attackStartEvent;
        private Request<AttackRequest> _attackRequest;

        public override void OnAwake()
        {
            _attackAnimationCompletionEvent = World.GetEvent<AttackAnimationCompletionEvent>();
            _attackStartEvent = World.GetEvent<AttackStartEvent>();
            _attackRequest = World.GetRequest<AttackRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var ent in _attackAnimationCompletionEvent.publishedChanges)
            {
                if (ent.Entity.IsNullOrDisposed()) continue;
                
                ref var unitComponent = ref ent.Entity.GetComponent<UnitComponent>(out var hasUnitComponent);
                ref var attackTargetComponent = ref ent.Entity.GetComponent<AttackTargetComponent>(out var hasAttackTarget);

                if (hasUnitComponent && hasAttackTarget && !attackTargetComponent.TargetEntity.IsNullOrDisposed())
                {
                    _attackStartEvent.NextFrame(new AttackStartEvent
                    {
                        AttackingEntity = ent.Entity,
                        AttackedEntity = attackTargetComponent.TargetEntity
                    });
                    
                    _attackRequest.Publish(new AttackRequest
                    {
                        AttackerEntity = ent.Entity,
                        TargetEntity = attackTargetComponent.TargetEntity,
                        AttackPosition = unitComponent.Value.Model.AttackPoint.position,
                        IsOriginal = true
                    });
                }
            }
        }
    }
}