using _Logic.Core;
using _Logic.Gameplay.Units.Effects.Requests;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Effects.Systems
{
    public class AreaImpactCreationRequestsProcessingSystem : AbstractUpdateSystem
    {
        private readonly Collider[] _colliders = new Collider[128];
        private Request<ImpactCreationRequest> _areaImpactCreationRequest;
        private Request<EffectAttachmentRequest> _impactActionAttachmentRequest;
        private LayerMask _defaultLayerMask = LayerMask.GetMask("Team0", "Team1");

        [Inject] private ImpactCatalog _impactCatalog;

        public override void OnAwake()
        {
            _areaImpactCreationRequest = World.GetRequest<ImpactCreationRequest>();
            _impactActionAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _areaImpactCreationRequest.Consume())
            {
                var teamDataComponent = request.InvokerEntity.GetComponent<TeamDataComponent>(out var hasTeamDataComponent);
                var data = _impactCatalog.GetData((int)request.Type);
                
                if (hasTeamDataComponent)
                {
                    var mask = 1 << (teamDataComponent.AlliesLayer | teamDataComponent.EnemiesLayer);
                    var colliderNumber = Physics.OverlapSphereNonAlloc(request.Position, data.Radius, _colliders, mask);

                    for (int i = 0; i < colliderNumber; i++)
                    {
                        if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) && !linkedCollider.Entity.IsNullOrDisposed())
                        {
                            var layer = _colliders[i].gameObject.layer;
                            
                            foreach (var effectType in data.EffectForAll)
                            {
                                _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                                {
                                    TargetEntity = linkedCollider.Entity,
                                    EffectType = effectType
                                });
                            }

                            if ((teamDataComponent.AlliesLayer & layer) == layer)
                            {
                                foreach (var effectType in data.EffectForAllies)
                                {
                                    _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                                    {
                                        TargetEntity = linkedCollider.Entity,
                                        EffectType = effectType
                                    });
                                }
                            }
                            
                            if ((teamDataComponent.EnemiesLayer & layer) == layer)
                            {
                                foreach (var effectType in data.EffectsForEnemies)
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
                }
            }
        }
    }
}