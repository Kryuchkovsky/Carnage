using _GameLogic.Extensions.Patterns;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Gameplay.Units.Experience
{
    public class PlayerExperienceBar : SingletonBehavior<PlayerExperienceBar>, IExperienceBar
    {
	    [SerializeField] private AnimationCurve _fillValueChangeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private Image _fillingImage;
        [SerializeField] private TextMeshProUGUI _experienceAmountText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private float _valueChangeDuration = 0.3f;

        private Tweener _fillValueChangeTweener;
        private float _currentFillValue;
        private float _targetFillValue;
        private bool _isInitiated;

        private void OnDestroy()
        {
	        _fillValueChangeTweener?.Kill();
        }

        protected override void Initialize()
        {
	        base.Initialize();
	        _fillValueChangeTweener = DOVirtual
		        .Float(_currentFillValue, _targetFillValue, _valueChangeDuration, t =>
		        {
			        _currentFillValue = t;
			        _fillingImage.fillAmount = _currentFillValue;
		        })
		        .SetEase(_fillValueChangeCurve)
		        .SetAutoKill(false)
		        .SetRecyclable(true)
		        .Pause();
	        _isInitiated = true;
	        SetFilling(1);
        }

        public void SetFilling(float filling)
        {
	        _targetFillValue = filling;
	        _currentFillValue = _currentFillValue > _targetFillValue ? 0 : _currentFillValue;
	        _fillValueChangeTweener.ChangeValues(_currentFillValue, _targetFillValue, _valueChangeDuration).Play();
        }
        
        public void SetExperienceAmount(float currentValue, float maxValue)
        {
	        _experienceAmountText.SetText("{0:0}/{1:0} exp.", currentValue, maxValue);
        }
        
        public void SetLevel(int level) => _levelText.SetText("{0:0} lv.", level);

        public void Reset()
		{
			transform.localScale = Vector3.one;
			_fillingImage.fillAmount = 1;
			_fillValueChangeTweener.Kill();
			_isInitiated = false;
		}
    }
}