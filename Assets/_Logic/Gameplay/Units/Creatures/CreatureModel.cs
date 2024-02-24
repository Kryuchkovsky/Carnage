using DG.Tweening;
using UnityEngine;

namespace _Logic.Gameplay.Units.Creatures
{
    public class CreatureModel : UnitModel
    {
        [SerializeField] private Animator _animator;

        private Sequence _jumpSequence;
        
        private readonly int _attackAnimationId = Animator.StringToHash("Attack");
        private readonly int _hitAnimationId = Animator.StringToHash("Hit");
        private readonly int _movementAnimationId = Animator.StringToHash("Speed_f");
        private readonly int _deathAnimationId = Animator.StringToHash("Death_b");

        private void Awake()
        {
            _jumpSequence = DOTween.Sequence()
                .Append(transform.DOLocalJump(Vector3.up, 1, 1, 0.25f))
                .Append(transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutQuad))
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }

        public override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
            _animator.SetTrigger(_attackAnimationId);
        }

        public override void PlayHitAnimation()
        {
            base.PlayHitAnimation();
            _animator.SetTrigger(_hitAnimationId);

            if (!_jumpSequence.IsPlaying())
            {
                //_jumpSequence.Restart();
            }
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