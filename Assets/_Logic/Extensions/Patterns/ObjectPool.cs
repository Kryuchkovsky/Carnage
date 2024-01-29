using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Logic.Extensions.Patterns
{
    public class ObjectPool<T> where T : Component
    {
        public event Action<T> CreationAction;
        public event Action<T> TakeAction;
        public event Action<T> ReturnAction;
    
        private readonly Stack<T> _objects;
        private readonly int _capacity;

        private Transform _storage;
        private int _totalCount;
        
        public T Prefab { get; private set; }

        public ObjectPool(T prefab, int capacity = 16, bool autoFilling = false, Transform poolParent = null, 
            Action<T> creationAction = null, Action<T> takeAction = null, Action<T> returnAction = null)
        {
            Prefab = prefab;
            _capacity = capacity;

            if (creationAction != null)
            {
                CreationAction += creationAction;
            }

            if (takeAction != null)
            {
                TakeAction += takeAction;
            }
        
            if (returnAction != null)
            {
                ReturnAction += returnAction;
            }
        
            _objects = new Stack<T>(_capacity);
            _storage = new GameObject(typeof(T) + "Pool").transform;
            _storage.parent = poolParent;
            Object.DontDestroyOnLoad(_storage);

            if (autoFilling)
            {
                FillPool();
            }
        }

        public T Take()
        {
            var obj = _objects.Count == 0 ? CreateObject() : _objects.Pop();
            TakeAction?.Invoke(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj)
        {
            _objects.Push(obj);
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_storage);
            obj.transform.localPosition = Vector3.zero;
            ReturnAction?.Invoke(obj);
        }

        private T CreateObject()
        {
            var obj = Object.Instantiate(Prefab);
            CreationAction?.Invoke(obj);
            return obj;
        }

        protected void FillPool()
        {
            while (_totalCount < _capacity)
            {
                var obj = CreateObject();
                obj.transform.SetParent(_storage);
                obj.gameObject.SetActive(false);
                _objects.Push(obj);
                _totalCount += 1;
            }
        }
    }
}