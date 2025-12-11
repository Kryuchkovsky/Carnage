using _GameLogic.Extensions;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Levels.Components;
using _Logic.Gameplay.SurvivalMode.Components;
using _Logic.Gameplay.SurvivalMode.Session;
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
    public sealed class EnemiesWavesHandlingSystem : AbstractUpdateSystem
    {
        private const int SpawnDistance = 50;
        
        private Filter _waveFilter;
        private Filter _playerFilter;
        private Filter _levelFilter;
        private Filter _unitCounterFilter;
        private Stash<WaveComponent> _waveStash;
        private Stash<TimerComponent> _timerStash;
        private Request<UnitSpawnRequest> _unitSpawnRequest;

        [Inject] private SessionService _sessionService;
        private SessionData _sessionData;
        
        [Inject] private SurvivalModeSettings _settings;
        private SpawnSettings _spawnSettings;

        public override void OnAwake()
        {
            _waveFilter = World.Filter.With<WaveComponent>().With<TimerComponent>().Build();
            _playerFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().Without<AIComponent>().Build();
            _levelFilter = World.Filter.With<LevelComponent>().With<BoundsComponent>().Build();
            _unitCounterFilter = World.Filter.With<UnitCounterComponent>().Build();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _waveStash = World.GetStash<WaveComponent>();
            _timerStash = World.GetStash<TimerComponent>();
            
            _sessionData = _sessionService.GetData();
            _spawnSettings = _settings.GetSpawnSettings(_sessionData.Difficulty);
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _waveFilter)
            {
                var waveTimer = _timerStash.Get(entity).Value;
                
                if (waveTimer <= 0)
                {
                    World.RemoveEntity(entity);
                    return;
                }

                var waveComponent = _waveStash.Get(entity);

                if (waveComponent.SpawnTimer > 0)
                {
                    waveComponent.SpawnTimer -= deltaTime;
                    continue;
                }

                var timeInWave = _spawnSettings.WaveDuration - waveTimer;
                waveComponent.SpawnTimer += CalculateSpawnRate(waveComponent.Wave, timeInWave);

                var levelEntity = _levelFilter.FirstOrDefault();
                var levelBoundsComponent = levelEntity.GetComponent<BoundsComponent>();
                
                var playerEntity = _playerFilter.FirstOrDefault();
                var unitCounterEntity = _unitCounterFilter.FirstOrDefault();
                
                if (playerEntity.IsNullOrDisposed() || unitCounterEntity.IsNullOrDisposed() || 
                    (unitCounterEntity.GetComponent<UnitCounterComponent>().TeamUnitNumbers.TryGetValue(1, out var number) && number >= _spawnSettings.MaxEnemiesNumber)) 
                    continue;
                
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
            }
        }
        
        private float CalculateSpawnRate(int wave, float timeInWave)
        {
            var waveProgress = timeInWave / _spawnSettings.WaveDuration;
            var baseRate = _spawnSettings.BaseSpawnRate * Mathf.Pow(1.2f, wave - 1);
            return baseRate * (1f + waveProgress * 2f);
        }
    }
}