using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Creatures;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Team;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class UnitSpawnRequestsHandlingSystem : AbstractSystem
    {
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private CreatureCatalog _creatureCatalog;

        public override void OnAwake()
        {
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _creatureCatalog = ConfigsManager.GetConfig<CreatureCatalog>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _unitSpawnRequest.Consume())
            {
                var data = _creatureCatalog.GetUnitData(request.UnitId);
                var creature = Object.Instantiate(_creatureCatalog.CreatureProvider, request.Position, Quaternion.identity);
                var model = Object.Instantiate(data.Model);
                creature.SetModel(model);
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
                creature.Entity.SetComponent(new ExperienceComponent
                {
                    Level = 1
                });

                World.GetRequest<TeamDataSettingRequest>().Publish(new TeamDataSettingRequest
                {
                    Entity = creature.Entity,
                    TeamId = request.TeamId
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