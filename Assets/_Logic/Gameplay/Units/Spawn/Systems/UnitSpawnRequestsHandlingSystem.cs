using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Experience;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Spawn.Components;
using _Logic.Gameplay.Units.Team;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class UnitSpawnRequestsHandlingSystem : AbstractSystem
    {
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        private Request<LevelChangeRequest> _levelChangeRequest;
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private UnitsCatalog _unitCatalog;
        private Transform _unitContainer;

        public override void OnAwake()
        {
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _unitCatalog = ConfigsManager.GetConfig<UnitsCatalog>();
            _unitContainer = new GameObject("UnitContainer").transform;
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _unitSpawnRequest.Consume())
            {
                var data = _unitCatalog.GetData((int)request.UnitType);
                var unit = Object.Instantiate(_unitCatalog.UnitProvider, request.Position, Quaternion.identity, _unitContainer);
                var model = Object.Instantiate(data.Model);
                unit.SetModel(model);

                if (data.TryGetData<AttackStats>(out var attackStats))
                {
                    unit.Entity.SetComponent(new AttackComponent
                    {
                        Stats = attackStats
                    });
                }
                
                if (data.TryGetData<HealthStats>(out var healthStats))
                {
                    unit.Entity.SetComponent(new HealthComponent
                    {
                        Stats = healthStats,
                        Value = healthStats.MaxHealth.CurrentValue
                    });
                }

                var hasMovementData = data.TryGetData<MovementStats>(out var movementData);
                var agentComponent = unit.Entity.GetComponent<NavMeshAgentComponent>(out var hasAgentComponent);
                var obstacleComponent = unit.Entity.GetComponent<NavMeshObstacleComponent>(out var hasObstacleComponent);
                
                if (hasMovementData)
                {
                    unit.Entity.SetComponent(new MovementComponent
                    {
                        Stats = movementData
                    });
                }

                agentComponent.Value.enabled = hasMovementData;
                obstacleComponent.Value.enabled = !hasMovementData;

                if (data.TryGetData<SpawnAbilityData>(out var spawnAbilityData))
                {
                    unit.Entity.SetComponent(new SpawnAbilityComponent
                    {
                        Data = spawnAbilityData
                    });
                    unit.Entity.AddComponent<TimerComponent>();
                }

                _levelChangeRequest.Publish(new LevelChangeRequest
                {
                    Entity = unit.Entity,
                    Change = 1
                });

                World.GetRequest<TeamDataSettingRequest>().Publish(new TeamDataSettingRequest
                {
                    Entity = unit.Entity,
                    TeamId = request.TeamId
                });

                if (request.IsPrioritizedTarget)
                {
                    unit.Entity.SetComponent(new PriorityComponent
                    {
                        Value = request.Priority
                    });
                }
                
                if (request.HasAI)
                {
                    unit.Entity.AddComponent<AIComponent>();
                }

                _unitSpawnEvent.NextFrame(new UnitSpawnEvent
                {
                    UnitProvider = unit,
                    Data = request
                });
            }
        }
    }
}