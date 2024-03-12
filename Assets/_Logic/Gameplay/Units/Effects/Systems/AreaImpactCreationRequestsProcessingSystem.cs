using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Effects;
using _Logic.Gameplay.Units.Effects.Requests;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Impacts.Systems
{
    public class AreaImpactCreationRequestsProcessingSystem : AbstractUpdateSystem
    {
        private readonly Collider[] _colliders = new Collider[128];
        private Request<ImpactCreationRequest> _areaImpactCreationRequest;
        private Request<EffectAttachmentRequest> _impactActionAttachmentRequest;
        private ImpactCatalog _impactCatalog;
        private LayerMask _defaultLayerMask = LayerMask.GetMask("Team0", "Team1");

        public override void OnAwake()
        {
            _areaImpactCreationRequest = World.GetRequest<ImpactCreationRequest>();
            _impactActionAttachmentRequest = World.GetRequest<EffectAttachmentRequest>();
            _impactCatalog = ConfigManager.GetConfig<ImpactCatalog>();
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
                            
                            foreach (var action in data.EffectForAll)
                            {
                                _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                                {
                                    TargetEntity = linkedCollider.Entity,
                                    Action = action
                                });
                            }

                            if ((teamDataComponent.AlliesLayer & layer) == layer)
                            {
                                foreach (var action in data.EffectForAllies)
                                {
                                    _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                                    {
                                        TargetEntity = linkedCollider.Entity,
                                        Action = action
                                    });
                                }
                            }
                            
                            if ((teamDataComponent.EnemiesLayer & layer) == layer)
                            {
                                foreach (var action in data.EffectsForEnemies)
                                {
                                    _impactActionAttachmentRequest.Publish(new EffectAttachmentRequest
                                    {
                                        TargetEntity = linkedCollider.Entity,
                                        Action = action
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