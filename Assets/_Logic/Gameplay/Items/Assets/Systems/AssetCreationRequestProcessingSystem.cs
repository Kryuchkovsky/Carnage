using System;
using System.Collections.Generic;
using _Logic.Core;
using _Logic.Extensions.Patterns;
using _Logic.Gameplay.Items.Assets.Components;
using _Logic.Gameplay.Items.Components;
using _Logic.Gameplay.Projectiles.Events;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.Items.Assets.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class AssetCreationRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<AssetCreationRequest> _experienceEssenceCreationRequest;
        private Request<ExperienceAmountChangeRequest> _experienceAmountChangeRequest;
        private Event<ProjectileFlightEndEvent> _projectileFlightEndEvent;
        private Stash<AssetComponent> _assetStash;
        private Dictionary<AssetType, ObjectPool<AssetProvider>> _assetPools;

        [Inject] private AssetsCatalog _assetsCatalog;
        
        public override void OnAwake()
        {
            _experienceEssenceCreationRequest = World.GetRequest<AssetCreationRequest>();
            _experienceAmountChangeRequest = World.GetRequest<ExperienceAmountChangeRequest>();
            _projectileFlightEndEvent = World.GetEvent<ProjectileFlightEndEvent>();
            _assetStash = World.GetStash<AssetComponent>();
            _assetPools = new Dictionary<AssetType, ObjectPool<AssetProvider>>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _experienceEssenceCreationRequest.Consume())
            {
                var provider = GetPool(request.Type).Take();
                provider.transform.position = request.Position;
                
                var data = _assetsCatalog.GetData(request.Type);

                _assetStash.Set(provider.Entity, new AssetComponent
                {
                    Provider = provider,
                    Data = data
                });
                
                switch (request.Type)
                {
                    case AssetType.None:
                        break;
                    case AssetType.Gold:
                        break;
                    case AssetType.Experience:
                        provider.Entity.SetComponent(new ExperienceAmountComponent
                        {
                            Value = request.Value
                        });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                provider.Entity.SetComponent(new CollectableComponent());
            }

            foreach (var evt in _projectileFlightEndEvent.publishedChanges)
            {
                if (evt.ProjectileEntity.IsNullOrDisposed() || !evt.ProjectileEntity.Has<AssetComponent>()) 
                    continue;

                ref var assetComponent = ref evt.ProjectileEntity.GetComponent<AssetComponent>();
                
                ref var experienceAmountComponent = ref evt.ProjectileEntity.GetComponent<ExperienceAmountComponent>(out var hasExperienceAmountComponent);

                if (hasExperienceAmountComponent)
                {
                    _experienceAmountChangeRequest.Publish(new ExperienceAmountChangeRequest
                    {
                        ReceivingEntity = evt.TargetEntity,
                        Change = experienceAmountComponent.Value
                    }, true);
                }

                GetPool(assetComponent.Data.Type).Return(assetComponent.Provider);
            }
        }

        private ObjectPool<AssetProvider> GetPool(AssetType type)
        {
            if (!_assetPools.ContainsKey(type))
            {
                var data = _assetsCatalog.GetData(type);
                _assetPools.Add(type, new ObjectPool<AssetProvider>(data.Prefab));
            }

            return _assetPools[type];
        }
    }
}