using UnityEngine;

namespace _Logic.Gameplay.Units.Creatures
{
    public class CreatureModel : UnitModel
    {
        [field: SerializeField] private Animator _animator;

        private readonly int _movementAnimationId = Animator.StringToHash("Speed_f");
        private readonly int _attackAnimationId = Animator.StringToHash("Attack");
        
        public override void PlayAttackAnimation()
        {
            _animator.SetTrigger(_attackAnimationId);
        }

        public override void SetMovementSpeed(float value)
        {
            _animator.SetFloat(_movementAnimationId, value);
        }
    }
}