using System.Collections.Generic;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Extensions.Patterns;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Events;
using _Logic.Gameplay.Units.Spawn.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Stats.Requests;
using _Logic.Gameplay.Units.Team;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class UnitSpawnRequestsHandlingSystem : AbstractUpdateSystem
    {
        private Dictionary<UnitType, ObjectPool<UnitProvider>> _objectPools;
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        private Request<LevelChangeRequest> _levelChangeRequest;
        private Request<TeamDataSettingRequest> _teamDataSettingRequest;
        private Request<StatDependentComponentsSetRequest> _statDependentComponentsSetRequest;
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private Event<UnitDeathEvent> _unitDeathEvent;
        private Filter _unitCounterFilter;
        private UnitsCatalog _unitCatalog;
        private Transform _unitContainer;

        public override void OnAwake()
        {
            _objectPools = new Dictionary<UnitType, ObjectPool<UnitProvider>>();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
            _teamDataSettingRequest = World.GetRequest<TeamDataSettingRequest>();
            _statDependentComponentsSetRequest = World.GetRequest<StatDependentComponentsSetRequest>();
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
            _unitCatalog = ConfigManager.Instance.GetConfig<UnitsCatalog>();
            _unitContainer = new GameObject("UnitContainer").transform;

            var entity = World.CreateEntity();
            entity.SetComponent(new UnitCounterComponent
            {
                TeamUnitNumbers = new Dictionary<int, int>()
            });

            _unitCounterFilter = World.Filter.With<UnitCounterComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _unitSpawnRequest.Consume())
            {
                if (!_objectPools.ContainsKey(request.UnitType))
                {
                    _objectPools.Add(request.UnitType, new ObjectPool<UnitProvider>(
                        prefab: _unitCatalog.UnitProvider));
                }
                
                var unit = _objectPools[request.UnitType].Take();
                unit.transform.parent = _unitContainer;
                unit.transform.position = request.Position;

                var entity = unit.Entity;
                var data = _unitCatalog.GetData((int)request.UnitType);
                
                if (unit.Model && unit.Model.Id == data.Model.Id)
                {
                    unit.Model.Reset();
                }
                else
                {
                    var model = Object.Instantiate(data.Model);
                    unit.SetModel(model);
                }
                
                unit.Entity.SetComponent(new UnitDataComponent
                {
                    Value = data
                });

                ref var statsComponent = ref entity.GetComponent<StatsComponent>(out var hasStatsComponent);
                
                if (hasStatsComponent)
                {
                    statsComponent.Value.Reset();
                }
                else
                {
                    var stats = new StatStorage();
                    
                    foreach (var statPair in data.Stats)
                    {
                        stats.Register(statPair.Key, statPair.Value);
                    }
                    
                    entity.SetComponent(new StatsComponent
                    {
                        Value = stats
                    });
                }

                _levelChangeRequest.Publish(new LevelChangeRequest
                {
                    Entity = entity,
                    Change = 1
                }, true);

                _teamDataSettingRequest.Publish(new TeamDataSettingRequest
                {
                    Entity = entity,
                    TeamId = request.TeamId
                }, true);
                
                _statDependentComponentsSetRequest.Publish(new StatDependentComponentsSetRequest
                {
                    Entity = unit.Entity
                }, true);

                if (request.IsPrioritizedTarget)
                {
                    unit.Entity.SetComponent(new PriorityComponent
                    {
                        Value = request.Priority
                    });
                }
                else if (unit.Entity.Has<PriorityComponent>())
                {
                    unit.Entity.RemoveComponent<PriorityComponent>();
                }

                if (request.HasAI && !unit.Entity.Has<AIComponent>())
                {
                    unit.Entity.AddComponent<AIComponent>();
                }
                else if (!request.HasAI && unit.Entity.Has<AIComponent>())
                {
                    unit.Entity.RemoveComponent<AIComponent>();
                }

                unit.Entity.AddComponent<AliveComponent>();
                
                _unitSpawnEvent.NextFrame(new UnitSpawnEvent
                {
                    Entity = unit.Entity
                });

                var unitNumbers = _unitCounterFilter.First().GetComponent<UnitCounterComponent>().TeamUnitNumbers;

                if (!unitNumbers.TryAdd(request.TeamId, 1))
                {
                    unitNumbers[request.TeamId] += 1;
                }
            }

            foreach (var deathEvent in _unitDeathEvent.publishedChanges)
            {
                var entity = deathEvent.CorpseEntity;
                
                if (entity.IsNullOrDisposed() || !entity.Has<UnitComponent>() || !entity.Has<UnitDataComponent>() || !entity.Has<TeamDataComponent>()) continue;

                ref var unitComponent = ref entity.GetComponent<UnitComponent>();
                ref var unitDataComponent = ref entity.GetComponent<UnitDataComponent>();
                ref var teamComponent = ref entity.GetComponent<TeamDataComponent>();
                unitComponent.Value.gameObject.layer = LayerMask.NameToLayer("Corpse");
                _objectPools[unitDataComponent.Value.Type].Return(unitComponent.Value, false);
                
                ref var counterComponent = ref _unitCounterFilter.First().GetComponent<UnitCounterComponent>();
                counterComponent.TeamUnitNumbers[teamComponent.Id] -= 1;
            }
        }
    }
}