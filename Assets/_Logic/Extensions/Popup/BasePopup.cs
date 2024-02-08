using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Logic.Extensions.Popup
{
    public abstract class BasePopup : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _text;
        [SerializeField] protected AnimationCurve _scaleCurve = new (
            new Keyframe(0, 1),
            new Keyframe(0.1f, 2),
            new Keyframe(0.2f, 1),
            new Keyframe(1, 1));
        
        [SerializeField, Range(0, 1)] protected float _duration = 0.5f;
        [SerializeField, Min(0)] protected float _velocity = 1;
        
        protected Sequence _animationSequence;
        protected bool _isInitiated;
    }
}