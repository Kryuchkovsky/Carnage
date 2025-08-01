﻿using _GameLogic.Extensions;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Levels.Components;
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
        
        private Filter _survivalModeFilter;
        private Filter _playerFilter;
        private Filter _levelFilter;
        private Filter _unitCounterFilter;
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        
        [Inject] private SurvivalModeSettings _settings;

        public override void OnAwake()
        {
            _survivalModeFilter = World.Filter.With<SurvivalModeComponent>().With<TimerComponent>().Build();
            _playerFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().Without<AIComponent>().Build();
            _levelFilter = World.Filter.With<LevelComponent>().With<BoundsComponent>().Build();
            _unitCounterFilter = World.Filter.With<UnitCounterComponent>().Build();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _survivalModeFilter)
            {
                if (entity.GetComponent<TimerComponent>().Value > 0) continue;

                var levelEntity = _levelFilter.FirstOrDefault();
                var levelBoundsComponent = levelEntity.GetComponent<BoundsComponent>();
                
                var playerEntity = _playerFilter.FirstOrDefault();
                var unitCounterEntity = _unitCounterFilter.FirstOrDefault();

                if (playerEntity.IsNullOrDisposed() || unitCounterEntity.IsNullOrDisposed() || 
                    (unitCounterEntity.GetComponent<UnitCounterComponent>().TeamUnitNumbers.TryGetValue(1, out var number) && number >= _settings.MaxEnemiesNumber)) continue;

                var position = playerEntity.GetComponent<TransformComponent>().Value.position;
                position += ExtraMethods.GetRandomDirectionXZ() * SpawnDistance;
                position = levelBoundsComponent.Value.ClosestPoint(position);
                
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