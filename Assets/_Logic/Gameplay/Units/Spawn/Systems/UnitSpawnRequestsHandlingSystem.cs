using System.Collections.Generic;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Experience;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Events;
using _Logic.Gameplay.Units.Movement;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Spawn.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Team;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class UnitSpawnRequestsHandlingSystem : AbstractUpdateSystem
    {
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        private Request<LevelChangeRequest> _levelChangeRequest;
        private Request<TeamDataSettingRequest> _teamDataSettingRequest;
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private Event<UnitDeathEvent> _unitDeathEvent;
        private UnitsCatalog _unitCatalog;
        private Transform _unitContainer;

        public override void OnAwake()
        {
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
            _teamDataSettingRequest = World.GetRequest<TeamDataSettingRequest>();
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
            _unitCatalog = ConfigManager.Instance.GetConfig<UnitsCatalog>();
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

                var stats = new Dictionary<StatType, Stat>();
                unit.Entity.SetComponent(new StatsComponent
                {
                    Value = stats
                });
                
                if (data.TryGetData<AttackStats>(out var attackStats))
                {
                    stats.Add(StatType.AttackTime, attackStats.AttackTime);
                    stats.Add(StatType.AttackDamage, attackStats.Damage);
                    stats.Add(StatType.AttackRange, attackStats.Range);
                    stats.Add(StatType.AttackSpeed, attackStats.Speed);
                    unit.Entity.SetComponent(new AttackComponent
                    {
                        Stats = attackStats
                    });
                }
                
                if (data.TryGetData<HealthStats>(out var healthStats))
                {
                    stats.Add(StatType.HealthRegenerationRate, healthStats.RegenerationRate);
                    stats.Add(StatType.MaxHeath, healthStats.MaxHealth);
                    stats.Add(StatType.CurrentHealth, healthStats.CurrentHealth);
                    unit.Entity.SetComponent(new HealthComponent
                    {
                        Stats = healthStats,
                    });
                }

                var hasMovementData = data.TryGetData<MovementStats>(out var movementData);
                var agentComponent = unit.Entity.GetComponent<NavMeshAgentComponent>(out var hasAgentComponent);
                var obstacleComponent = unit.Entity.GetComponent<NavMeshObstacleComponent>(out var hasObstacleComponent);
                
                if (hasMovementData)
                {
                    stats.Add(StatType.MovementSpeed, movementData.MovementSpeed);
                    stats.Add(StatType.RotationSpeed, movementData.RotationSpeed);
                    unit.Entity.SetComponent(new MovementComponent
                    {
                        Stats = movementData
                    });
                }

                if (hasAgentComponent)
                {
                    agentComponent.Value.enabled = hasMovementData;
                }

                if (hasObstacleComponent)
                {
                    obstacleComponent.Value.enabled = !hasMovementData;
                }

                if (data.TryGetData<SpawnAbilityData>(out var spawnAbilityData))
                {
                    stats.Add(StatType.AbilityCooldownSpeed, spawnAbilityData.SpawnInterval);
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
                }, true);

                _teamDataSettingRequest.Publish(new TeamDataSettingRequest
                {
                    Entity = unit.Entity,
                    TeamId = request.TeamId
                }, true);

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
                else
                {
                    unit.Entity.SetComponent(new ExperienceBarComponent
                    {
                        Value = PlayerExperienceBar.Instance
                    });
                    unit.Entity.SetComponent(new StatsPanelComponent
                    {
                        Value = StatsPanel.Instance
                    });

                    StatsPanel.Instance.Initiate(stats);
                }

                _unitSpawnEvent.NextFrame(new UnitSpawnEvent
                {
                    Entity = unit.Entity
                });
            }

            foreach (var deathEvent in _unitDeathEvent.publishedChanges)
            {
                if (deathEvent.CorpseEntity.IsNullOrDisposed() || !deathEvent.CorpseEntity.Has<UnitComponent>()) continue;

                ref var unitComponent = ref deathEvent.CorpseEntity.GetComponent<UnitComponent>();
                
                unitComponent.Value.OnDie();
                deathEvent.CorpseEntity.Dispose();
            }
        }
    }
}