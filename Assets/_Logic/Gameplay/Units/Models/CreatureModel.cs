using System;
using _Logic.Gameplay.Units.Attack;
using DG.Tweening;
using UnityEngine;

namespace _Logic.Gameplay.Units.Models
{
    public class CreatureModel : UnitModel
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _attackEffect;

        private AttackStateMachineBehavior[] _attackStateMachineBehaviors;
        private Sequence _jumpSequence;
        private Action _attackAnimationCallback;

        private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
        private readonly int _attackSpeedFloatHash = Animator.StringToHash("AttackSpeed");
        private readonly int _weaponTypeIntegerHash = Animator.StringToHash("WeaponType_int");
        private readonly int _hitTriggerHash = Animator.StringToHash("Hit");
        private readonly int _movementSpeedFloatHash = Animator.StringToHash("Speed_f");
        private readonly int _deathBooleanHash = Animator.StringToHash("Death_b");
        
        [field: SerializeField] public WeaponType WeaponType { get; private set; }

        private void Awake()
        {
            _attackStateMachineBehaviors = _animator.GetBehaviours<AttackStateMachineBehavior>();

            foreach (var behavior in _attackStateMachineBehaviors)
            {
                behavior.AttackCompleted += InvokeAttackAnimationCallback;
            }
            
            _jumpSequence = DOTween.Sequence()
                .Append(transform.DOLocalJump(Vector3.up, 1, 1, 0.25f))
                .Append(transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutQuad))
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
            _animator.SetInteger(_weaponTypeIntegerHash, (int)WeaponType);
        }

        private void OnDestroy()
        {
            foreach (var behavior in _attackStateMachineBehaviors)
            {
                behavior.AttackCompleted -= InvokeAttackAnimationCallback;
            }
        }

        public override void PlayAttackAnimation(float attackSpeed = 1, Action callback = null)
        {
            _animator.SetFloat(_attackSpeedFloatHash, attackSpeed);
            _animator.SetTrigger(_attackTriggerHash);
            _attackAnimationCallback = callback;
            
            if (_attackEffect) 
                _attackEffect.Play();
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

        public void Hit()
        {
            InvokeAttackAnimationCallback();
        }
        
        public void Shoot()
        {
            InvokeAttackAnimationCallback();
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

        public override void LookAtPoint(Vector3 point, float rotationSpeed)
        {
            base.LookAtPoint(point, rotationSpeed);
            var direction = (point - transform.position).normalized;
            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        }

        public override void Reset()
        {
            base.Reset();
            _animator.SetBool(_deathBooleanHash, false);
            transform.rotation = Quaternion.identity;
        }

        private void InvokeAttackAnimationCallback()
        {
            if (_attackAnimationCallback == null) 
                return;
            
            _attackAnimationCallback.Invoke();
            _attackAnimationCallback = null;
        }
    }
}