using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Logic.Extensions.Patterns
{
    public class ObjectPool<T> where T : Component
    {
        private Action<T> _creationAction;
        private Action<T> _takeAction;
        private Action<T> _returnAction;
    
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
                _creationAction += creationAction;
            }

            if (takeAction != null)
            {
                _takeAction += takeAction;
            }
        
            if (returnAction != null)
            {
                _returnAction += returnAction;
            }
        
            _objects = new Stack<T>(_capacity);
            _storage = new GameObject(typeof(T) + "Pool").transform;

            if (poolParent)
            {
                _storage.parent = poolParent; 
            }

            Object.DontDestroyOnLoad(_storage);

            if (autoFilling)
            {
                FillPool();
            }
        }

        public T Take()
        {
            var obj = _objects.Count == 0 ? CreateObject() : _objects.Pop();
            _takeAction?.Invoke(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj, bool disabling = true)
        {
            _objects.Push(obj);
            obj.gameObject.SetActive(!disabling);

            if (obj.transform.parent.gameObject.activeInHierarchy)
            {
                obj.transform.SetParent(_storage);
            }

            if (disabling) 
                obj.transform.localPosition = Vector3.zero;
            
            _returnAction?.Invoke(obj);
        }

        private T CreateObject()
        {
            var obj = Object.Instantiate(Prefab, _storage);
            _creationAction?.Invoke(obj);
            return obj;
        }

        private void FillPool()
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