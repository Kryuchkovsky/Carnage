using System.Collections.Generic;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.VFXManager;
using _Logic.Gameplay.Effects.Components;
using _Logic.Gameplay.Effects.Requests;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Effects.Systems
{
    public class ImpactCreationRequestsProcessingSystem : AbstractUpdateSystem
    {
        private Request<ImpactCreationRequest> _impactCreationRequest;
        private Transform _container;

        [Inject] private VFXService _vfxService;
        [Inject] private ImpactCatalog _impactCatalog;

        public override void OnAwake()
        {
            _impactCreationRequest = World.GetRequest<ImpactCreationRequest>();
            _container = new GameObject("ImpactContainer").transform;
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _impactCreationRequest.Consume())
            {
                var teamComponent = request.Invoker.GetComponent<TeamComponent>(out var hasTeam);
                var data = _impactCatalog.GetData((int)request.Type);
                
                var impactObject = Object.Instantiate(_impactCatalog.ImpactPrefab, request.Position, Quaternion.identity, _container);
                impactObject.Entity.SetComponent(new ImpactDataComponent
                {
                    ImpactedEntities = new HashSet<Entity>(),
                    Data = data,
                    LastCheckTime = data.Duration
                });
                impactObject.Entity.SetComponent(new TimerComponent
                {
                    Value = data.Duration
                });
                
                if (hasTeam)
                {
                    impactObject.Entity.SetComponent(teamComponent);
                }
                
                _vfxService.CreateEffect(data.VFXType, request.Position, Quaternion.identity);
            }
        }
    }
}