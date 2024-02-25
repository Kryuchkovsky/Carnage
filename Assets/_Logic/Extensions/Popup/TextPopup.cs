using DG.Tweening;
using UnityEngine;

namespace _Logic.Extensions.Popup
{
    public class TextPopup : BasePopup
    {
        private Camera _camera;
        
        public void Setup(Camera camera, Color color, string value, float scale, TweenCallback callback = null)
        {
            _text.SetText(value);
            Setup(camera, color, scale, callback);
        }
        
        public void Setup(Camera camera, Color color, float value, float scale, TweenCallback callback = null)
        {
            _text.SetText("{0:0}", value);
            Setup(camera, color, scale, callback);
        }
        
        private void Setup(Camera camera, Color color, float scale, TweenCallback callback = null)
        {
            _camera = camera;
            transform.localScale = Vector3.zero;
            _text.color = color;
            var targetPositionY = transform.position.y + _duration * _velocity;
            
            _animationSequence?.Kill();
            _animationSequence = DOTween.Sequence()
                .Append(transform.DOMoveY(targetPositionY, _duration).SetEase(Ease.Linear))
                .Join(transform.DOScale(Vector3.one * scale, _duration).SetEase(_scaleCurve))
                .Join(_text.DOFade(0, _duration).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    _isInitiated = false;
                });

            _isInitiated = true;
        }
    }
}