using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Extensions.HealthBar
{
	public class HealthBarView : MonoBehaviour
	{
		[SerializeField] private Canvas _canvas;
		[SerializeField] private AnimationCurve _fillValueChangeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		[SerializeField] private Image _damageImage;
		[SerializeField] private Image _healingImage;
		[SerializeField] private Image _currentHpImage;
		[SerializeField] private float _offset = 1f;
		[SerializeField] private float _activityChangeDuration = 0.3f;
		[SerializeField] private float _valueChangeDuration = 0.3f;
		
		private Camera _camera;
		private Sequence _openingSequence;
		private Sequence _closingSequence;
		private Tweener _fillValueChangeTweener;
		private float _currentFillValue;
		private float _targetFillValue;
		private bool _isInitiated;

		protected void Awake()
		{
			_fillValueChangeTweener = DOVirtual
				.Float(_currentFillValue, _targetFillValue, _valueChangeDuration, t =>
				{
					_currentFillValue = t;
					_currentHpImage.fillAmount = _targetFillValue > _currentFillValue ? _currentFillValue : _targetFillValue;
					_damageImage.fillAmount = _targetFillValue < _currentFillValue ? _currentFillValue : _targetFillValue;
					Debug.Log(_currentFillValue);
				})
				.SetEase(_fillValueChangeCurve)
				.SetAutoKill(false)
				.SetRecyclable(true);
			_fillValueChangeTweener.Pause();
		}

		private void LateUpdate()
		{
			if (_isInitiated)
			{
				transform.rotation = _camera.transform.rotation;
			}
		}

		private void OnDestroy()
		{
			_openingSequence?.Kill();
			_closingSequence?.Kill();
			_fillValueChangeTweener?.Kill();
		}

		public HealthBarView Initiate(Camera camera, HealthBarColorData data, Transform parent, float heightOffset)
		{
			_camera = camera;
			_canvas.worldCamera = _camera;
			_damageImage.color = data.DamageColor;
			_healingImage.color = data.HealingColor;
			_currentHpImage.color = data.BaseColor;

			transform.parent = parent;
            var targetPosition = Vector3.zero;
            targetPosition.y = heightOffset + _offset;
            transform.localPosition = targetPosition;
            SetFillValue(1, true);

            _isInitiated = true;

            return this;
        }

		public HealthBarView SetActivity(bool isActive, bool isImmediate = false)
		{
			_closingSequence.Kill();
			_openingSequence.Kill();
			
			if (isImmediate)
			{
				gameObject.SetActive(isActive);
				transform.localScale = isActive ? Vector3.one : Vector3.zero;
			}
			else if (isActive)
			{
				_openingSequence = DOTween.Sequence()
					.AppendCallback(() => gameObject.SetActive(true))
					.Append(transform.DOScale(1, _activityChangeDuration).SetEase(Ease.OutBack));
			}
			else
			{
				_closingSequence = DOTween.Sequence()
					.Append(transform.DOScale(0, _activityChangeDuration).SetEase(Ease.InBack))
					.AppendCallback(() => gameObject.SetActive(false));
			}

			return this;
		}

		public HealthBarView SetFillValue(float value, bool isImmediate = false)
		{
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