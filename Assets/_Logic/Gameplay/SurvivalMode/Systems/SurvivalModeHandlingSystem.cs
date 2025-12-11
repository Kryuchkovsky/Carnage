using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.SurvivalMode.Components;
using _Logic.Gameplay.SurvivalMode.Session;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SurvivalModeHandlingSystem : AbstractUpdateSystem
    {
        private Stash<WaveComponent> _waveStash;
        private Stash<TimerComponent> _timerStash;

        [Inject] private SessionService _sessionService;
        private SessionData _sessionData;
        
        [Inject] private SurvivalModeSettings _settings;
        private SpawnSettings _spawnSettings;

        public override void OnAwake()
        {
            _waveStash = World.GetStash<WaveComponent>();
            _timerStash = World.GetStash<TimerComponent>();

            _sessionData = _sessionService.GetData();
            _spawnSettings = _settings.GetSpawnSettings(_sessionData.Difficulty);
        }

        public override void OnUpdate(float deltaTime)
        {
            _sessionData.GameTime += deltaTime;
            _sessionData.TimeBeforeWaweSpawn -= deltaTime;

            if (_sessionData.TimeBeforeWaweSpawn <= 0)
            {
                _sessionData.WaveCount += 1;
                
                var waveEntity = World.CreateEntity();
                _waveStash.Set(waveEntity, new WaveComponent
                {
                    Wave = _sessionData.WaveCount
                });
                _timerStash.Set(waveEntity, new TimerComponent
                {
                    Value = _spawnSettings.WaveDuration
                });

                _sessionData.TimeBeforeWaweSpawn = _spawnSettings.WaveDuration;
            }
        }
    }
}