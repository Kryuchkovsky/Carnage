using TMPro;
using UnityEngine;

namespace _Logic.Extensions.Popup
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private AnimationCurve _movementCurve = new(
            new Keyframe(0, 0),
            new Keyframe(0.3f, 1),
            new Keyframe(0.5f, 0.9f),
            new Keyframe(1, -1));

        [SerializeField] private AnimationCurve _scaleCurve = new(
            new Keyframe(0, 1),
            new Keyframe(0.1f, 2),
            new Keyframe(0.2f, 1),
            new Keyframe(1, 1));

        [SerializeField] private AnimationCurve _alphaCurve = new(
            new Keyframe(0, 1),
            new Keyframe(0.3f, 1),
            new Keyframe(1, 0));

        [SerializeField, Range(0, 1)] private float _duration = 0.5f;
        [SerializeField, Min(0)] private float _velocity = 1;

        private Camera _camera;
        private Transform _target;
        private Vector3 _worldPosition;
        private Vector3 _endPosition;
        private float _time;

        [field: SerializeField, HideInInspector]
        public string Format { get; set; } = "{0:0}";

        [field: SerializeField, HideInInspector]
        public int Id { get; set; }

        public bool IsCompleted { get; private set; }

        public void Initialize(Vector3 position, Camera camera = null)
        {
            _time = 0;
            _camera = camera;

            if (_camera)
            {
                _worldPosition = position;
            }
            
            _endPosition = Vector3.up * _duration * _velocity;
            IsCompleted = false;
        }

        public void Initialize(Camera camera, Transform target)
        {
            _target = target;
            Initialize(_target.position, camera);
        }

        public void Setup(Color color, string value)
        {
            _text.SetText(value);
            _text.color = color;
        }

        public void Setup(Color color, float value)
        {
            _text.SetText(Format, value);
            _text.color = color;
        }

        public void UpdateTime(float delta)
        {
            if (IsCompleted) return;

            if (_time >= _duration)
            {
                _time = 0;
                _target = null;
                _camera = null;
                IsCompleted = true;
            }
            else
            {
                _time += delta;
            }

            var progress = _time / _duration;
            var position = _endPosition * _movementCurve.Evaluate(progress);

            if (_camera)
            {
                if (_target)
                {
                    _worldPosition = _target.position;
                }

                position += _camera.WorldToScreenPoint(_worldPosition);
            }

            transform.position = position;

            var scale = Vector3.one * _scaleCurve.Evaluate(progress);
            transform.localScale = scale;

            var color = _text.color;
            color.a = _alphaCurve.Evaluate(progress);
            _text.color = color;
        }
    }
}