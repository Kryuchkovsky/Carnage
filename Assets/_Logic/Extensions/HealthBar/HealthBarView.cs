using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Extensions.HealthBar
{
	public class HealthBarView : MonoBehaviour
	{
		[SerializeField] private AnimationCurve _fillValueChangeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		[SerializeField] private Image _damageImage;
		[SerializeField] private Image _healingImage;
		[SerializeField] private Image _currentHpImage;
		[SerializeField] private float _baseOffset = 1f;
		[SerializeField] private float _activityChangeDuration = 0.3f;
		[SerializeField] private float _valueChangeDuration = 0.3f;

		private Camera _camera;
		private Sequence _openingSequence;
		private Sequence _closingSequence;
		private Tweener _fillValueChangeTweener;
		private float _currentFillValue;
		private float _targetFillValue;
		private float _hidingDelay;
		private float _timeBeforeHiding;
		private float _additionalOffset;
		private bool _isInitiated;
		private bool _hasTemporaryShowing;
		private Transform _target;

		protected void Awake()
		{
			_fillValueChangeTweener = DOVirtual
				.Float(_currentFillValue, _targetFillValue, _valueChangeDuration, t =>
				{
					_currentFillValue = t;
					_currentHpImage.fillAmount = _targetFillValue > _currentFillValue ? _currentFillValue : _targetFillValue;
					_damageImage.fillAmount = _targetFillValue < _currentFillValue ? _currentFillValue : _targetFillValue;
				})
				.SetEase(_fillValueChangeCurve)
				.SetAutoKill(false)
				.SetRecyclable(true);
			_fillValueChangeTweener.Pause();
		}

		private void OnDestroy()
		{
			_openingSequence?.Kill();
			_closingSequence?.Kill();
			_fillValueChangeTweener?.Kill();
		}

		public HealthBarView Initiate(Camera camera, HealthBarColorData data, Transform target, float heightOffset, float hidingDelay)
		{
			_additionalOffset = heightOffset;
			_camera = camera;
			_target = target;
			_damageImage.color = data.DamageColor;
			_healingImage.color = data.HealingColor;
			_currentHpImage.color = data.BaseColor;
			_hidingDelay = hidingDelay;
			_hasTemporaryShowing = hidingDelay > 0;
			SetFillValue(1, true);
			_isInitiated = true;

            return this;
        }

		public void Update()
		{
			if (!_isInitiated) return;

			if (_hasTemporaryShowing)
			{
				if (_timeBeforeHiding > 0)
				{
					_timeBeforeHiding -= Time.deltaTime;
				}
				
				SetActivity(_timeBeforeHiding > 0, true);
			}
			
			if ((_hasTemporaryShowing && _timeBeforeHiding > 0) || !_hasTemporaryShowing)
			{
				transform.position = _camera.WorldToScreenPoint(_target.position + Vector3.up * (_baseOffset + _additionalOffset));
			}
		}

		public HealthBarView SetActivity(bool isActive, bool isImmediate = false)
		{
			if (isImmediate)
			{
				_closingSequence.Kill();
				_openingSequence.Kill();
				gameObject.SetActive(isActive);
				transform.localScale = isActive ? Vector3.one : Vector3.zero;
			}
			else if (isActive)
			{
				_closingSequence.Kill();

				if (_openingSequence != null && _openingSequence.IsPlaying()) return this;
				
				_openingSequence = DOTween.Sequence()
					.AppendCallback(() => gameObject.SetActive(true))
					.Append(transform.DOScale(1, _activityChangeDuration).SetEase(Ease.OutBack));
			}
			else
			{
				_openingSequence.Kill();

				if (_closingSequence != null && _closingSequence.IsPlaying()) return this;
				
				_closingSequence = DOTween.Sequence()
					.Append(transform.DOScale(0, _activityChangeDuration).SetEase(Ease.InBack))
					.AppendCallback(() => gameObject.SetActive(false));
			}

			return this;
		}

		public HealthBarView SetFillValue(float value, bool isImmediate = false)
		{
			if (_hasTemporaryShowing)
			{
				_timeBeforeHiding = _hidingDelay;
			}
			
			if (!gameObject.activeInHierarchy) return this;
			
			_targetFillValue = value;
			_healingImage.fillAmount = _targetFillValue;
			_currentFillValue = isImmediate ? value : _currentFillValue;

			var duration = isImmediate ? _valueChangeDuration : 0.1f;
			_fillValueChangeTweener.ChangeValues(_currentFillValue, _targetFillValue, duration);
			_fillValueChangeTweener.Play();

			return this;
		}

		public HealthBarView Reset()
		{
			transform.localScale = Vector3.one;
			_currentHpImage.fillAmount = 1;
			_damageImage.fillAmount = 1;
			_healingImage.fillAmount = 1;
			_fillValueChangeTweener.Kill();
			_openingSequence.Kill();
			_closingSequence.Kill();
			_isInitiated = false;

			return this;
		}
	}
}