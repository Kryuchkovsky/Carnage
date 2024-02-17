using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Creatures;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class CreatureSpawnRequestsHandlingSystem : AbstractSystem
    {
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private CreaturesCatalog _creaturesCatalog;

        public override void OnAwake()
        {
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _creaturesCatalog = ConfigsManager.GetConfig<CreaturesCatalog>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _unitSpawnRequest.Consume())
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
                    BacisData = data.MovementData,
                    CurrentData = data.MovementData
                });
                creature.Entity.SetComponent(new TeamIdComponent
                {
                    Value = request.TeamId
                });
                creature.Entity.SetComponent(new ExperienceComponent
                {
                    Level = 1
                });

                if (string.IsNullOrEmpty(data.AttackData.ProjectileData.Id))
                {
                    creature.Entity.AddComponent<MeleeAttackComponent>();
                }
                else
                {
                    creature.Entity.AddComponent<RangeAttackComponent>();
                }

                if (request.HasAI)
                {
                    creature.Entity.AddComponent<AIComponent>();
                }
                
                _unitSpawnEvent.NextFrame(new UnitSpawnEvent
                {
                    UnitProvider = creature,
                    Data = request
                });
            }
        }
    }
}