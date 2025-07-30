using Scellecs.Morpeh;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Logic.Core
{
    public abstract class EcsBootstrapper : MonoBehaviour
    {
        [SerializeField] private LifetimeScope _lifetimeScope;
        [SerializeField] private int _order;
        
        private SystemsGroup _systemsGroup;

        public abstract World World { get; }

        private bool _isRegistered;

        private void OnEnable()
        {
            _systemsGroup = World.CreateSystemsGroup();
            
            RegisterSystems(_lifetimeScope.Container);
            _isRegistered = true;
            
            World.AddSystemsGroup(_order, _systemsGroup);
        }
        
        public EcsBootstrapper AddInitializer<T>() where T : class, IInitializer, new()
        {
            var instance = new T() as IInitializer;
            _lifetimeScope.Container.Inject(instance);
            _systemsGroup.AddInitializer(instance);
            
            return this;
        }

        public EcsBootstrapper AddInitializer(IInitializer instance)
        {
            _lifetimeScope.Container.Inject(instance);
            _systemsGroup.AddInitializer(instance);
            
            return this;
        }
        
        public EcsBootstrapper AddSystem<T>() where T : class, ISystem, new()
        {
            var instance = new T() as ISystem;
            _lifetimeScope.Container.Inject(instance);
            _systemsGroup.AddSystem(instance);

            return this;
        }
        
        public EcsBootstrapper AddSystem(ISystem system)
        {
            _lifetimeScope.Container.Inject(system);
            _systemsGroup.AddSystem(system);

            return this;
        }

        private void OnDisable()
        {
            World.RemoveSystemsGroup(_systemsGroup);
        }
        
        protected abstract void RegisterSystems(IObjectResolver resolver);
    }
}