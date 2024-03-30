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
    public class ItemCollectionSystem : QuerySystem
    {
        private readonly Collider[] _colliders = new Collider[32];
        private readonly LayerMask _mask = LayerMask.NameToLayer("Item");
        private readonly float _checkInterval = 1;
        private float _time;

        protected override void Configure()
        {
            _time -= deltaTime;
            
            if (_time > 0) return;
            
            CreateQuery()
                .With<CollectorComponent>().With<TransformComponent>().With<AliveComponent>()
                .ForEach((Entity entity, ref CollectorComponent collectorComponent, ref TransformComponent transformComponent) =>
                {
                    var position = transformComponent.Value.position;
                    var mask = 1 << _mask;
                    var itemsNumber = Physics.OverlapSphereNonAlloc(position, collectorComponent.Radius, _colliders, mask);

                    for (int i = 0; i < itemsNumber; i++)
                    {
                        if (_colliders[i].TryGetComponent<ItemProvider>(out var item) && item.Entity.Has<CollectableComponent>())
                        {
                            item.Entity.RemoveComponent<CollectableComponent>();
                            
                            item.Entity.SetComponent(new TargetComponent
                            {
                                Entity = entity
                            });
                        }
                    }
                });

            _time += _checkInterval;
        }
    }
}