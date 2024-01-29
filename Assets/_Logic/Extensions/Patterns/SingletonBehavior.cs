using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace _GameLogic.Extensions.Patterns
{
    public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
    {
        [SerializeField] private bool _dontDestroyOnLoad;

        private static T _instance;

        private bool _isInitiated;

        public static bool Instantiated
        {
            get
            {
                if (Application.isPlaying)
                {
                    return _instance != null;
                }

                return FindObjectsOfType<T>().Length == 1;
            }
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (!_instance._isInitiated)
                    {
                        _instance.Init();
                    }
                }

                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                DestroyImmediate(this);
                return;
            }

            _instance = this as T;
            
            if (!_isInitiated)
            {
                Init();
            }

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }

        public void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        protected virtual void Init()
        {
            _isInitiated = true;
        }
    }
}