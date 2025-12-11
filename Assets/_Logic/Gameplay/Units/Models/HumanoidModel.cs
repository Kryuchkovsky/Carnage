using System;
using System.Collections.Generic;
using System.Linq;
using _Logic.Gameplay.Equipment;
using _Logic.Gameplay.Equipment.Weapon;
using _Logic.Gameplay.Items;
using _Logic.Gameplay.Units.Attack;
using DG.Tweening;
using UnityEngine;

namespace _Logic.Gameplay.Units.Models
{
    public class HumanoidModel : UnitModel
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private List<ItemSlot> _itemPlaces;

        private Dictionary<SlotType, ItemSlot> _slotsCache;
        private AttackStateMachineBehavior[] _attackStateMachineBehaviors;
        private Sequence _jumpSequence;
        private WeaponProvider _weapon;
        private Action _attackAnimationCallback;

        private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
        private readonly int _attackSpeedFloatHash = Animator.StringToHash("AttackSpeed");
        private readonly int _hitTriggerHash = Animator.StringToHash("Hit");
        private readonly int _movementSpeedFloatHash = Animator.StringToHash("Speed_f");
        private readonly int _deathBooleanHash = Animator.StringToHash("Death_b");
        private readonly int _weaponTypeHash = Animator.StringToHash("WeaponType");

        private void Awake()
        {
            _attackStateMachineBehaviors = _animator.GetBehaviours<AttackStateMachineBehavior>();
            _slotsCache = _itemPlaces.ToDictionary(s => s.SlotType, s => s);
            
            foreach (var behavior in _attackStateMachineBehaviors)
                behavior.AttackCompleted += InvokeAttackAnimationCallback;
            
            _jumpSequence = DOTween.Sequence()
                .Append(transform.DOLocalJump(Vector3.up, 1, 1, 0.25f))
                .Append(transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutQuad))
                .SetAutoKill(false)
                .SetRecyclable(true)
                .Pause();
        }

        private void OnDestroy()
        {
            foreach (var behavior in _attackStateMachineBehaviors)
            {
                behavior.AttackCompleted -= InvokeAttackAnimationCallback;
            }
        }

        public override Transform GetAttackPoint()
        {
            if (_weapon)
                return _weapon.AttackPoint;
            
            return base.GetAttackPoint();
        }

        public override void PlayAttackAnimation(float attackSpeed = 1, Action callback = null)
        {
            _animator.SetFloat(_attackSpeedFloatHash, attackSpeed);
            _animator.SetTrigger(_attackTriggerHash);
            _attackAnimationCallback = callback;
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

        public bool GetEquipment(SlotType slotType, out EquipmentProvider equipment)
        {
            var itemPlace = _itemPlaces.FirstOrDefault(s => s.SlotType == slotType);

            if (itemPlace != null)
                return itemPlace.Get(out equipment);

            equipment = null;
            return false;
        }
        
        public void SetEquipment(SlotType slotType, EquipmentType equipmentType, EquipmentProvider equipment)
        {
            if (equipment is WeaponProvider weapon)
            {
                _weapon = weapon;
                _animator.SetInteger(_weaponTypeHash, (int)equipmentType);
            }

            if (_slotsCache.TryGetValue(slotType, out var slot))
                slot.Set(equipment);
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
            _attackAnimationCallback?.Invoke();
            _attackAnimationCallback = null;
        }
    }
}