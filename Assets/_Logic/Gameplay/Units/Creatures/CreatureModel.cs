using UnityEngine;

namespace _Logic.Gameplay.Units.Creatures
{
    public class CreatureModel : UnitModel
    {
        [field: SerializeField] private Animator _animator;

        private readonly int _attackAnimationId = Animator.StringToHash("Attack");
        private readonly int _hitAnimationId = Animator.StringToHash("Hit");
        private readonly int _movementAnimationId = Animator.StringToHash("Speed_f");
        private readonly int _deathAnimationId = Animator.StringToHash("Death_b");

        public override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
            _animator.SetTrigger(_attackAnimationId);
        }

        public override void PlayHitAnimation()
        {
            base.PlayHitAnimation();
            _animator.SetTrigger(_hitAnimationId);
        }

        public override void PlayDeathAnimation()
        {
            base.PlayDeathAnimation();
            _animator.SetBool(_deathAnimationId, true);
        }

        public override void SetMovementSpeed(float value)
        {
            base.SetMovementSpeed(value);
            _animator.SetFloat(_movementAnimationId, value);
        }
    }
}