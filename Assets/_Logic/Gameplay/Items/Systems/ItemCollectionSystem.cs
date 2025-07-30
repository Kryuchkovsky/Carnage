using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Items.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Items.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class ItemCollectionSystem : AbstractUpdateSystem
    {
        private Filter _collectorFilter;
        private Stash<CollectorComponent> _collectorStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<CollectableComponent> _collectableStash;
        private Stash<TargetComponent> _targetStash;
        private readonly Collider[] _colliders = new Collider[32];
        private readonly LayerMask _mask = LayerMask.NameToLayer("Item");
        private readonly float _checkInterval = 1;
        private float _time;

        public override void OnAwake()
        {
            _collectorFilter = World.Filter.With<CollectorComponent>().With<TransformComponent>().With<AliveComponent>().Build();
            _collectorStash = World.GetStash<CollectorComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _collectableStash = World.GetStash<CollectableComponent>();
            _targetStash = World.GetStash<TargetComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            _time -= deltaTime;
            
            if (_time > 0) return;
            
            foreach (var entity in _collectorFilter)
            {
                ref var collectorComponent = ref _collectorStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                
                var position = transformComponent.Value.position;
                var mask = 1 << _mask;
                var itemsNumber = Physics.OverlapSphereNonAlloc(position, collectorComponent.Radius, _colliders, mask);

                for (int i = 0; i < itemsNumber; i++)
                {
                    if (_colliders[i].TryGetComponent<ItemProvider>(out var item) && _collectableStash.Has(entity))
                    {
                        _collectableStash.Remove(entity);
                            
                        _targetStash.Set(entity, new TargetComponent
                        {
                            Entity = entity
                        });
                    }
                }
            }
            
            _time += _checkInterval;
        }
    }
}