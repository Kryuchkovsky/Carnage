using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Patterns;
using _Logic.Gameplay.Items.Components;
using _Logic.Gameplay.Items.Requests;
using _Logic.Gameplay.Projectiles.Events;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.Items.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class ExperienceEssenceCreationRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<ExperienceEssenceCreationRequest> _experienceEssenceCreationRequest;
        private Request<ExperienceAmountChangeRequest> _experienceAmountChangeRequest;
        private Event<ProjectileFlightEndEvent> _projectileFlightEndEvent;
        private ObjectPool<ItemProvider> _experienceEssencePool;

        [Inject] private ItemConfig _itemConfig;
        
        public override void OnAwake()
        {
            _experienceEssenceCreationRequest = World.GetRequest<ExperienceEssenceCreationRequest>();
            _experienceAmountChangeRequest = World.GetRequest<ExperienceAmountChangeRequest>();
            _projectileFlightEndEvent = World.GetEvent<ProjectileFlightEndEvent>();
            
            var data = _itemConfig.GetItemData(ItemType.Experience);
            _experienceEssencePool = new ObjectPool<ItemProvider>(data.Prefab);
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _experienceEssenceCreationRequest.Consume())
            {
                var provider = _experienceEssencePool.Take();
                provider.transform.position = request.Position;
                provider.Entity.SetComponent(_itemConfig.FlightParameters);
                provider.Entity.SetComponent(new ExperienceAmountComponent
                {
                    Value = request.ExperienceAmount
                });

                if (!provider.Entity.Has<CollectableComponent>())
                {
                    provider.Entity.AddComponent<CollectableComponent>();
                }
            }

            foreach (var evt in _projectileFlightEndEvent.publishedChanges)
            {
                if (evt.ProjectileEntity.IsNullOrDisposed() || !evt.ProjectileEntity.Has<ItemComponent>() || !evt.ProjectileEntity.Has<ExperienceAmountComponent>()) continue;

                ref var experienceAmountComponent = ref evt.ProjectileEntity.GetComponent<ExperienceAmountComponent>();
                
                _experienceAmountChangeRequest.Publish(new ExperienceAmountChangeRequest
                {
                    ReceivingEntity = evt.TargetEntity,
                    Change = experienceAmountComponent.Value
                }, true);
                
                ref var itemComponent = ref evt.ProjectileEntity.GetComponent<ItemComponent>();
                _experienceEssencePool.Return(itemComponent.Provider);
            }
        }
    }
}