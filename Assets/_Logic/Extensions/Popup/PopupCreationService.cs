using _GameLogic.Extensions.Patterns;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.Popup
{
    public class PopupCreationService : SingletonBehavior<PopupCreationService>
    {
        private ObjectPool<TextPopup> _textPopupsPool;

        [SerializeField] private Camera _camera;
        [SerializeField] private TextPopup _textPopup;
        [SerializeField] private float _popupOffsetY = 3;
        [SerializeField] private float _popupDistance = 4;

        protected override void Init()
        {
            base.Init();
            _camera ??= Camera.main;
            _textPopupsPool = new(_textPopup, 16, true);
        }

        public void CreateWorldTextPopup(Transform target, string text, Color color = default, float scale = 1)
        {
            var popup = _textPopupsPool.Take();
            var direction = (_camera.transform.position - target.position).normalized;
            var position = target.position + Vector3.up * _popupOffsetY + direction * _popupDistance;
            popup.transform.position = position;
            popup.Setup(_camera, color, text, scale, () => _textPopupsPool.Return(popup));
        }
    }
}