using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Systems
{
    public class UnitSpawnRequestsHandlingSystem : AbstractSystem
    {
        private Request<UnitSpawnRequest> _request;
        private UnitCatalog _unitCatalog;

        public override void OnAwake()
        {
            _request = World.GetRequest<UnitSpawnRequest>();
            _unitCatalog = ConfigsManager.GetConfig<UnitCatalog>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                var prefab = _unitCatalog.GetUnit(request.UnitId);
                var createdUnit = Object.Instantiate(prefab, request.Position, Quaternion.identity);
                createdUnit.Entity.SetComponent(new TeamIdComponent
                {
                    Value = request.TeamId
                });
                createdUnit.Entity.SetComponent(new DestinationComponent
                {
                    Value = Vector3.zero
                });
            }
        }
    }
}