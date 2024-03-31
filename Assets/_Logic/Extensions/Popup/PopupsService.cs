using System.Collections.Generic;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.Popup
{
    public class PopupsService : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Popup popupPrefab;

        private Dictionary<int, ObjectPool<Popup>> _pools;
        private List<Popup> _activePopups;
        private int _generalPopupId;

        protected void Awake()
        {
            _camera ??= Camera.main;
            _pools = new Dictionary<int, ObjectPool<Popup>>();
            _activePopups = new List<Popup>(1024);
            _generalPopupId = RegisterPopupAndGetId();
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _activePopups.Count;)
            {
                _activePopups[i].UpdateTime(Time.deltaTime);
                
                if (_activePopups[i].IsCompleted)
                {
                    _pools[_activePopups[i].Id].Return(_activePopups[i]);
                }
                else
                {
                    i++;
                }
            }
        }

        public int RegisterPopupAndGetId(string format = "{0:0}")
        {
            var prefab = Instantiate(popupPrefab, _canvas.transform);
            prefab.gameObject.SetActive(false);
            prefab.Format = format;
            var id = AddPoolAndGetPoolId(prefab);
            prefab.Id = id;
            return id;
        }

        public void CreateTextPopup(Transform target, string text, Color color = default) =>
            CreateTextPopup(_generalPopupId, target, text, color);
        
        public void CreateTextPopup(Transform target, float value, Color color = default) =>
            CreateTextPopup(_generalPopupId, target, value, color);
        
        public void CreateTextPopup(Vector3 worldPosition, string text, Color color = default) =>
            CreateTextPopup(_generalPopupId, worldPosition, text, color);
        
        public void CreateTextPopup(Vector3 worldPosition, float value, Color color = default) =>
            CreateTextPopup(_generalPopupId, worldPosition, value, color);

        public void CreateTextPopup(int id, Transform target, string text, Color color = default)
        {
            var popup = _pools[id].Take();
            popup.Initialize(_camera, target);
            popup.Setup(color, text);
        }
        
        public void CreateTextPopup(int id, Transform target, float value, Color color = default)
        {
            var popup = _pools[id].Take();
            popup.Initialize(_camera, target);
            popup.Setup(color, value);
        }
        
        public void CreateTextPopup(int id, Vector3 worldPosition, string text, Color color = default)
        {
            var popup = _pools[id].Take();
            popup.Initialize(worldPosition, _camera);
            popup.Setup(color, text);
        }
        
        public void CreateTextPopup(int id, Vector3 worldPosition, float value, Color color = default)
        {
            var popup = _pools[id].Take();
            popup.Initialize(worldPosition, _camera);
            popup.Setup(color, value);
        }

        private int AddPoolAndGetPoolId(Popup prefab)
        {
            var id = _pools.Count;
            var pool = new ObjectPool<Popup>(prefab, 8, true, _canvas.transform,
                takeAction: p => _activePopups.Add(p),
                returnAction: p => _activePopups.Remove(p));
            _pools.Add(id, pool);
            return id;
        }
    }
}