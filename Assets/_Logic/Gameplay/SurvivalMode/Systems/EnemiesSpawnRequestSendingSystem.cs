using _GameLogic.Extensions;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.SurvivalMode.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Spawn;
using _Logic.Gameplay.Units.Spawn.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemiesSpawnRequestSendingSystem : AbstractUpdateSystem
    {
        private const int SpawnDistance = 50;
        
        private FilterBuilder _survivalModeFilter;
        private FilterBuilder _playerFilter;
        private FilterBuilder _unitCounterFilter;
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        
        [Inject] private SurvivalModeSettings _settings;

        public override void OnAwake()
        {
            _survivalModeFilter = World.Filter.With<SurvivalModeComponent>().With<TimerComponent>();
            _playerFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().Without<AIComponent>();
            _unitCounterFilter = World.Filter.With<UnitCounterComponent>();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _survivalModeFilter.Build())
            {
                if (entity.GetComponent<TimerComponent>().Value > 0) continue;

                var playerEntity = _playerFilter.Build().FirstOrDefault();
                var unitCounterEntity = _unitCounterFilter.Build().FirstOrDefault();

                if (playerEntity.IsNullOrDisposed() || unitCounterEntity.IsNullOrDisposed() || 
                    (unitCounterEntity.GetComponent<UnitCounterComponent>().TeamUnitNumbers.TryGetValue(1, out var number) && number >= _settings.MaxEnemiesNumber)) continue;

                var position = playerEntity.GetComponent<TransformComponent>().Value.position;
                position += ExtraMethods.GetRandomDirectionXZ() * SpawnDistance;
                var allUnitTypes = _settings.Enemies;
                var unitType = allUnitTypes[Random.Range(0, allUnitTypes.Count)];

                _unitSpawnRequest.Publish(new UnitSpawnRequest
                {
                    UnitType = unitType,
                    Position = position,
                    TeamId = 1,
                    HasAI = true
                });
                entity.SetComponent(new TimerComponent
                {
                    Value = _settings.EnemySpawnInterval
                });
            }
        }
    }
}