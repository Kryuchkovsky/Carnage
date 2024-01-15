using _Logic.Core.Components;
using _Logic.Gameplay.Units.Creatures.Components;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Gameplay.Units.Creatures
{
    public class CreatureProvider : UnitProvider
    {
        [SerializeField] private Animator _animator;
        [SerializeField, CanBeNull] private NavMeshAgent _navMeshAgent;

        private int _movementAnimationId;
        private int _attackAnimationId;
        
        protected override void Initialize()
        {
            base.Initialize();

            _movementAnimationId = Animator.StringToHash("Speed_f");
            _attackAnimationId = Animator.StringToHash("Attack");
            
            Entity.SetComponent(new CreatureComponent()
            {
                Value = this
            });
            
            if (_navMeshAgent != null)
            {
                Entity.SetComponent(new NavMeshAgentComponent
                {
                    Value = _navMeshAgent
                });
            }
        }

        public override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
            _animator.SetTrigger(_attackAnimationId);
        }

        public override void SetMovementSpeed(float value)
        {
            base.SetMovementSpeed(value);
            _animator.SetFloat(_movementAnimationId, value);
        }
    }
}