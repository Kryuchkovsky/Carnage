using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.Popup
{
    public class PopupsService : SingletonBehavior<PopupsService>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextPopup _textPopup;
        [SerializeField] private float _popupOffsetY = 3;

        private ObjectPool<TextPopup> _textPopupsPool;
        private HashSet<BasePopup> _activePopups;
        
        protected override void Initialize()
        {
            base.Initialize();
            _camera ??= Camera.main;
            _textPopupsPool = new(_textPopup, 16, true, _canvas.transform,
                takeAction: p => _activePopups.Add(p),
                returnAction: p => _activePopups.Remove(p));
            _activePopups = new HashSet<BasePopup>(1024);
        }

        private void LateUpdate()
        {
            foreach (var popup in _activePopups)
            {
                popup.Update();
            }
        }

        public void CreateWorldTextPopup(Transform target, string text, Color color = default, float scale = 1)
        {
            var popup = _textPopupsPool.Take();
            var position = target.position + Vector3.up * _popupOffsetY;
            popup.transform.position = _camera.WorldToScreenPoint(position);
            popup.Setup(_camera, color, text, scale, () => _textPopupsPool.Return(popup));
        }
    }
}