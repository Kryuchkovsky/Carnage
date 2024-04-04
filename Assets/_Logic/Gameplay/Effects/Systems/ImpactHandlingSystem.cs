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
    public class ImpactHandlingSystem : QuerySystem
    {
        private readonly Collider[] _colliders = new Collider[128];
        private Request<EffectAttachmentRequest> _impactActionAttachmentRequest;
        private readonly float _checkInterval = 0.25f;

        [Inject] private ImpactCatalog _impactCatalog;

        public override void OnAwake()
        {
            base.OnAwake();
            _impactActionAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
        }
        
        protected override void Configure()
        {
            CreateQuery()
                .With<ImpactComponent>().With<ImpactDataComponent>().With<TeamComponent>().With<TimerComponent>().With<TransformComponent>()
                .ForEach((Entity entity, ref ImpactDataComponent impactComponent, ref TeamComponent teamComponent, ref TimerComponent timerComponent, 
                    ref TransformComponent transformComponent) =>
                {
                    if (timerComponent.Value > 0 && impactComponent.LastCheckTime - timerComponent.Value < _checkInterval) return;
                    
                    impactComponent.LastCheckTime = timerComponent.Value;
                    
                    var progress = 1 - timerComponent.Value / impactComponent.Data.Duration;
                    impactComponent.Progress = progress;
                    
                    var radius = impactComponent.Data.CalculateRadius(progress);
                    var mask = (1 << teamComponent.AlliesLayer) | (1 << teamComponent.EnemiesLayer);
                    var colliderNumber = Physics.OverlapSphereNonAlloc(transformComponent.Value.position, radius, _colliders, mask);
                    
                    for (int i = 0; i < colliderNumber; i++)
                    {
                        if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) && !linkedCollider.Entity.IsNullOrDisposed() && 
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
                });
        }
    }
}