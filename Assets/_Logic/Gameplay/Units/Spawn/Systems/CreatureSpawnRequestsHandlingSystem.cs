using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class CreatureSpawnRequestsHandlingSystem : AbstractSystem
    {
        private Request<UnitSpawnRequest> _request;
        private CreaturesCatalog _creaturesCatalog;

        public override void OnAwake()
        {
            _request = World.GetRequest<UnitSpawnRequest>();
            _creaturesCatalog = ConfigsManager.GetConfig<CreaturesCatalog>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                var data = _creaturesCatalog.GetUnitData(request.UnitId);
                var creature = Object.Instantiate(_creaturesCatalog.CreatureProvider, request.Position, Quaternion.identity);
                var model = Object.Instantiate(data.Model);
                creature.SetModel(model);
                creature.SetColor(request.TeamId == 0 ? Color.blue : Color.red);
                creature.Entity.SetComponent(new AttackComponent
                {
                    BacisData = data.AttackData,
                    CurrentData = data.AttackData,
                });
                creature.Entity.SetComponent(new HealthComponent
                {
                    BasicData = data.HealthData,
                    CurrentData = data.HealthData,
                    Value = data.HealthData.MaxValue
                });
                creature.Entity.SetComponent(new MovementComponent
                {
                    BacisData = data.MovementData
                });
                creature.Entity.SetComponent(new TeamIdComponent
                {
                    Value = request.TeamId
                });
                creature.Entity.SetComponent(new DestinationComponent
                {
                    Value = Vector3.zero
                });
                creature.Entity.SetComponent(new ExperienceComponent
                {
                    Level = 1
                });

                if (request.HasAI)
                {
                    creature.Entity.AddComponent<AIComponent>();
                }
                
                World.GetEvent<UnitSpawnEvent>().NextFrame(new UnitSpawnEvent
                {
                    UnitProvider = creature,
                    Data = request
                });
            }
        }
    }
}