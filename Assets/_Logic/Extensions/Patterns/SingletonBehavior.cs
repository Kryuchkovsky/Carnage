using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace _GameLogic.Extensions.Patterns
{
    public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
    {
        [SerializeField] private bool _dontDestroyOnLoad;

        private static T _instance;

        public bool IsInitiated { get; private set; }

        public static bool Instantiated
        {
            get
            {
                if (Application.isPlaying)
                {
                    return _instance != null;
                }

                return FindObjectsByType<T>(FindObjectsSortMode.InstanceID).Length == 1;
            }
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();

                    if (!_instance.IsInitiated)
                    {
                        _instance.Initialize();
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
            
            if (!IsInitiated)
            {
                Initialize();
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

        protected virtual void Initialize()
        {
            IsInitiated = true;
        }
    }
}