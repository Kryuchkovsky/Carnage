using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Effects.Components;
using _Logic.Gameplay.Effects.Requests;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Effects.Systems
{
    public class ImpactHandlingSystem : AbstractUpdateSystem
    {
        private readonly Collider[] _colliders = new Collider[128];
        private Request<EffectAttachmentRequest> _impactActionAttachmentRequest;
        private Filter _impactFilter;
        private Stash<ImpactDataComponent> _impactDataStash;
        private Stash<TeamComponent> _teamStash;
        private Stash<TimerComponent> _timerStash;
        private Stash<TransformComponent> _transformStash;
        private readonly float _checkInterval = 0.25f;

        [Inject] private ImpactCatalog _impactCatalog;

        public override void OnAwake()
        {
            _impactActionAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
            _impactFilter = World.Filter.With<ImpactComponent>().With<ImpactDataComponent>().With<TeamComponent>()
                .With<TimerComponent>().With<TransformComponent>().Build();
            _impactDataStash = World.GetStash<ImpactDataComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _timerStash = World.GetStash<TimerComponent>();
            _transformStash = World.GetStash<TransformComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _impactFilter)
            {
                ref var impactComponent = ref _impactDataStash.Get(entity); 
                ref var teamComponent = ref _teamStash.Get(entity);
                ref var timerComponent = ref _timerStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                
                if (timerComponent.Value > 0 &&
                    impactComponent.LastCheckTime - timerComponent.Value < _checkInterval) return;

                impactComponent.LastCheckTime = timerComponent.Value;

                var progress = 1 - timerComponent.Value / impactComponent.Data.Duration;
                impactComponent.Progress = progress;

                var radius = impactComponent.Data.CalculateRadius(progress);
                var mask = (1 << teamComponent.AlliesLayer) | (1 << teamComponent.EnemiesLayer);
                var colliderNumber =
                    Physics.OverlapSphereNonAlloc(transformComponent.Value.position, radius, _colliders, mask);

                for (int i = 0; i < colliderNumber; i++)
                {
                    if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) &&
                        !linkedCollider.Entity.IsNullOrDisposed() &&
                        impactComponent.ImpactedEntities.Add(linkedCollider.Entity))
                    {
                        var layer = _colliders[i].gameObject.layer;

                        foreach (var effectType in impactComponent.Data.EffectForAll)
                        {
                            _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                            {
                                TargetEntity = linkedCollider.Entity,
                                EffectType = effectType
                            });
                        }

                        if ((teamComponent.AlliesLayer & layer) == layer)
                        {
                            foreach (var effectType in impactComponent.Data.EffectForAllies)
                            {
                                _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                                {
                                    TargetEntity = linkedCollider.Entity,
                                    EffectType = effectType
                                });
                            }
                        }

                        if ((teamComponent.EnemiesLayer & layer) == layer)
                        {
                            foreach (var effectType in impactComponent.Data.EffectsForEnemies)
                            {
                                _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                                {
                                    TargetEntity = linkedCollider.Entity,
                                    EffectType = effectType
                                });
                            }
                        }
                    }
                }

                if (timerComponent.Value <= 0)
                {
                    Object.Destroy(transformComponent.Value.gameObject);
                    entity.Dispose();
                }
            }
        }
    }
}