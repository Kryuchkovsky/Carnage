using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;

namespace _GameLogic.Core
{
    public abstract class EcsBootstrapper : MonoBehaviour
    {
        [SerializeField] private int _order;
        
        private HashSet<Initializer> _initializers;
        private HashSet<UpdateSystem> _updateSystems;
        private HashSet<FixedUpdateSystem> _fixedUpdateSystems;
        private HashSet<LateUpdateSystem> _lateUpdateSystems;
        private HashSet<CleanupSystem> _cleanupSystems;
        private SystemsGroup _systemsGroup;

        public abstract World World { get; }

        private bool isRegistered;

        private void OnEnable()
        {
            _initializers = new HashSet<Initializer>();
            _updateSystems = new HashSet<UpdateSystem>();
            _fixedUpdateSystems = new HashSet<FixedUpdateSystem>();
            _lateUpdateSystems = new HashSet<LateUpdateSystem>();
            _cleanupSystems = new HashSet<CleanupSystem>();
            _systemsGroup = World.CreateSystemsGroup();
            
            RegisterSystems();
            isRegistered = true;
            
            World.AddSystemsGroup(_order, _systemsGroup);
        }

        private void OnDisable()
        {
            World.RemoveSystemsGroup(_systemsGroup);
        }
        
        protected abstract void RegisterSystems();

        public EcsBootstrapper AddInitializer<T>() where T : IInitializer, new()
        {
            if (Application.isPlaying && !isRegistered)
            {
                RemoveAll();
            }
            
            var instance = new T() as Initializer;

            if (!_initializers.Contains(instance))
            {
                _initializers.Add(instance);
            }
            
            if (Application.isPlaying && !isRegistered)
            {
                AddAll();
            }

            return this;
        }

        public EcsBootstrapper AddSystem<T>() where T : ISystem, new()
        {
            if (Application.isPlaying && !isRegistered)
            {
                RemoveAll();
            }
            
            var instance = new T();

            if (instance is UpdateSystem updateSystem && !_updateSystems.Contains(updateSystem))
            {
                _updateSystems.Add(updateSystem);
            }
            else if (instance is FixedUpdateSystem fixedUpdateSystem && !_fixedUpdateSystems.Contains(fixedUpdateSystem))
            {
                _fixedUpdateSystems.Add(fixedUpdateSystem);
            }
            else if (instance is LateUpdateSystem lateUpdateSystem && !_lateUpdateSystems.Contains(lateUpdateSystem))
            {
                _lateUpdateSystems.Add(lateUpdateSystem);
            }
            else if (instance is CleanupSystem cleanupSystem && !_cleanupSystems.Contains(cleanupSystem))
            {
                _cleanupSystems.Add(cleanupSystem);
            }
            else
            {
                throw new Exception("The system with the same type already has been registered!");
            }

            if (Application.isPlaying && !isRegistered)
            {
                AddAll();
            }

            return this;
        }
        
        private void AddAll()
        {
            foreach (var initializer in _initializers)
            {
                _systemsGroup.AddInitializer(initializer);
            }

            foreach (var system in _updateSystems)
            {
                _systemsGroup.AddSystem(system);
            }

            foreach (var system in _fixedUpdateSystems)
            {
                _systemsGroup.AddSystem(system);
            }
            
            foreach (var system in _lateUpdateSystems)
            {
                _systemsGroup.AddSystem(system);
            }
            
            foreach (var system in _cleanupSystems)
            {
                _systemsGroup.AddSystem(system);
            }
        }
        
        private void RemoveAll()
        {
            foreach (var initializer in _initializers)
            {
                _systemsGroup.RemoveInitializer(initializer);
            }

            foreach (var system in _updateSystems)
            {
                _systemsGroup.RemoveSystem(system);
            }
            
            foreach (var system in _fixedUpdateSystems)
            {
                _systemsGroup.RemoveSystem(system);
            }
            
            foreach (var system in _lateUpdateSystems)
            {
                _systemsGroup.RemoveSystem(system);
            }
            
            foreach (var system in _cleanupSystems)
            {
                _systemsGroup.RemoveSystem(system);
            }
        }
    }
}