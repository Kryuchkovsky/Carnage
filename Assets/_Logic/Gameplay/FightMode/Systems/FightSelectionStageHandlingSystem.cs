using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.FightMode.Components;
using _Logic.Gameplay.SurvivalMode;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Spawn;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.FightMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class FightSelectionStageHandlingSystem : AbstractInitializationSystem
    {
        private Filter _fightModeFilter;
        private Filter _playerUnitFilter;
        private Filter _gameCameraFilter;
        private Filter _levelFilter;
        private Stash<GameCameraComponent> _gameCameraStash;
        private Stash<UnitComponent> _unitStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<TeamComponent> _teamStash;
        private Stash<PriorityComponent> _priorityStash;
        private Stash<TimerComponent> _timerStash;
        private Stash<FightModeComponent> _fightModeStash;
        private Stash<FightLevelComponent> _fightLevelStash;
        private Request<UnitSpawnRequest> _unitSpawnRequest;

        [Inject] private MenuUIContainer _menuUIContainer;
        [Inject] private GameplayUIContainer _gameplayUIContainer;
        [Inject] private ConfigManager _configManager;
        private FightModeSettings Settings => _configManager.GetConfig<FightModeSettings>();

        public override void OnAwake()
        {
            _fightModeFilter = World.Filter.With<FightModeComponent>().Build();
            _playerUnitFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<TeamComponent>().With<PriorityComponent>().Without<AIComponent>().Build();
            _gameCameraFilter = World.Filter.With<GameCameraComponent>().Build();
            _levelFilter = World.Filter.With<FightLevelComponent>().Build();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _gameCameraStash = World.GetStash<GameCameraComponent>();
            _unitStash = World.GetStash<UnitComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _priorityStash = World.GetStash<PriorityComponent>();
            _timerStash = World.GetStash<TimerComponent>();
            _fightModeStash = World.GetStash<FightModeComponent>();
            _fightLevelStash = World.GetStash<FightLevelComponent>();
            _menuUIContainer.SetActivity(true);
            _gameplayUIContainer.SetActivity(false);
            _menuUIContainer.NextHeroButton.onClick.AddListener(SwitchToNextHero);
            _menuUIContainer.PreviousHeroButton.onClick.AddListener(SwitchToPreviousHero);
            _menuUIContainer.StartButton.onClick.AddListener(StartGame);

            foreach (var entity in _gameCameraFilter)
            {
                ref var cameraComponent = ref _gameCameraStash.Get(entity);
                cameraComponent.Value.SetMenuCamera();
            }
            
            var levelEntity = _levelFilter.FirstOrDefault();
            ref var levelComponent = ref _fightLevelStash.Get(levelEntity);
            var levelProvider = levelComponent.Value;
            levelProvider.TryGetSpawnPosition(0, out var spawnPosition1);
            
            _unitSpawnRequest.Publish(new UnitSpawnRequest
            {
                UnitType = Settings.Heroes[0],
                Position = spawnPosition1,
                LookDirection = Vector3.forward,
                TeamId = 0,
                HasAI = false,
            });
        }

        private void SwitchToNextHero() => ChangeUnitTypes(1);
        private void SwitchToPreviousHero() => ChangeUnitTypes(-1);

        private void StartGame()
        {
            var levelEntity = _levelFilter.FirstOrDefault();
            ref var levelComponent = ref _fightLevelStash.Get(levelEntity);
            var levelProvider = levelComponent.Value;
            
            foreach (var entity in _fightModeFilter)
                _timerStash.Set(entity);
            
            foreach (var entity in _gameCameraFilter)
            {
                ref var cameraComponent = ref _gameCameraStash.Get(entity);
                cameraComponent.Value.SetFightCamera();
            }

            levelProvider.TryGetSpawnPosition(0, out var spawnPosition1);
            levelProvider.TryGetSpawnPosition(1, out var spawnPosition2);
            
            for (int i = 0; i < Settings.MaxUnitsInArmy; i++)
            {
                _unitSpawnRequest.Publish(new UnitSpawnRequest
                {
                    UnitType = Settings.GetRandomUnit(),
                    Position = spawnPosition1,
                    LookDirection = Vector3.forward,
                    TeamId = 0,
                    HasAI = true
                });
            }
            
            for (int i = 0; i < Settings.MaxUnitsInArmy; i++)
            {
                _unitSpawnRequest.Publish(new UnitSpawnRequest
                {
                    UnitType = Settings.GetRandomUnit(),
                    Position = spawnPosition2,
                    LookDirection = Vector3.back,
                    TeamId = 1,
                    HasAI = true
                });
            }
            
            _menuUIContainer.SetActivity(false);
            _gameplayUIContainer.SetActivity(true);
        }

        private void ChangeUnitTypes(int change)
        {
            foreach (var modeEntity in _fightModeFilter)
            {
                ref var fightModeComponent = ref _fightModeStash.Get(modeEntity);
                var id = fightModeComponent.HeroId + change;

                if (id >= Settings.Heroes.Count)
                {
                    id -= Settings.Heroes.Count;
                }
                else if (id < 0)
                {
                    id = Settings.Heroes.Count - 1;
                }
                
                fightModeComponent.HeroId = id;

                foreach (var unitEntity in _playerUnitFilter)
                {
                    var type = Settings.Heroes[fightModeComponent.HeroId];

                    ref var unitComponent = ref _unitStash.Get(unitEntity);
                    ref var transformComponent = ref _transformStash.Get(unitEntity);
                    ref var teamComponent = ref _teamStash.Get(unitEntity);
                    ref var priorityComponent = ref _priorityStash.Get(unitEntity);
                    
                    _unitSpawnRequest.Publish(new UnitSpawnRequest
                    {
                        UnitType = type,
                        Position = transformComponent.Value.position,
                        LookDirection = Vector3.back,
                        TeamId = teamComponent.Id,
                        Priority = priorityComponent.Value
                    });

                    Object.Destroy(unitComponent.Value.gameObject);
                    World.RemoveEntity(unitEntity);
                }
            }
        }
    }
}