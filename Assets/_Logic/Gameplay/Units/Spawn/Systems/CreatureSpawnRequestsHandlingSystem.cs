using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Creatures.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public class CreatureSpawnRequestsHandlingSystem : AbstractSystem
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
                var data = _unitCatalog.GetUnitData(request.UnitId);
                var createdUnit = Object.Instantiate(data.Prefab, request.Position, Quaternion.identity);
                createdUnit.Entity.SetComponent(new BasicUnitDataComponent
                {
                    Value = data
                });
                createdUnit.Entity.SetComponent(new AttackComponent
                {
                    BacisData = data.AttackData,
                    CurrentData = data.AttackData,
                });
                createdUnit.Entity.SetComponent(new HealthComponent
                {
                    BasicData = data.HealthData,
                    CurrentData = data.HealthData
                });
                createdUnit.Entity.SetComponent(new MovementComponent
                {
                    BacisData = data.MovementData
                });
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