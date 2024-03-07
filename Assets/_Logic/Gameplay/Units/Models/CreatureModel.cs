using _Logic.Gameplay.Units.Attack;
using DG.Tweening;
using UnityEngine;

namespace _Logic.Gameplay.Units.Models
{
    public class CreatureModel : UnitModel
    {
        [SerializeField] private Animator _animator;

        private Sequence _jumpSequence;

        private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
        private readonly int _weaponTypeIntegerHash = Animator.StringToHash("WeaponType_int");
        private readonly int _hitTriggerHash = Animator.StringToHash("Hit");
        private readonly int _movementSpeedFloatHash = Animator.StringToHash("Speed_f");
        private readonly int _deathBooleanHash = Animator.StringToHash("Death_b");
        
        [field: SerializeField] public WeaponType WeaponType { get; private set; }

        private void Awake()
        {
            _jumpSequence = DOTween.Sequence()
                .Append(transform.DOLocalJump(Vector3.up, 1, 1, 0.25f))
                .Append(transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutQuad))
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
            _animator.SetInteger(_weaponTypeIntegerHash, (int)WeaponType);
        }

        public override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
            _animator.SetTrigger(_attackTriggerHash);
        }

        public override void PlayHitAnimation()
        {
            base.PlayHitAnimation();
            _animator.SetTrigger(_hitTriggerHash);

            if (!_jumpSequence.IsPlaying())
            {
                //_jumpSequence.Restart();
            }
        }

        public override void PlayDeathAnimation()
        {
            base.PlayDeathAnimation();
            _animator.SetBool(_deathBooleanHash, true);
        }

        public override void SetMovementSpeed(float value)
        {
            base.SetMovementSpeed(value);
            _animator.SetFloat(_movementSpeedFloatHash, value);
        }

        public override void LookAtPoint(Vector3 point)
        {
            base.LookAtPoint(point);
            var direction = (point - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}