using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.SurvivalMode.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Spawn;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class HeroSelectionStageHandlingSystem : AbstractInitializationSystem
    {
        private Filter _survivalModeFilter;
        private Filter _playerUnitFilter;
        private Filter _gameCameraFilter;
        private Stash<GameCameraComponent> _gameCameraStash;
        private Stash<UnitComponent> _unitStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<TeamComponent> _teamStash;
        private Stash<PriorityComponent> _priorityStash;
        private Stash<TimerComponent> _timerStash;
        private Stash<SurvivalModeComponent> _survivalModeStash;
        private Request<UnitSpawnRequest> _unitSpawnRequest;

        [Inject] private MenuUIContainer _menuUIContainer;
        [Inject] private GameplayUIContainer _gameplayUIContainer;
        [Inject] private SurvivalModeSettings _settings;

        public override void OnAwake()
        {
            _survivalModeFilter = World.Filter.With<SurvivalModeComponent>().Build();
            _playerUnitFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<TeamComponent>().With<PriorityComponent>().Without<AIComponent>().Build();
            _gameCameraFilter = World.Filter.With<GameCameraComponent>().Build();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _gameCameraStash = World.GetStash<GameCameraComponent>();
            _unitStash = World.GetStash<UnitComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _priorityStash = World.GetStash<PriorityComponent>();
            _timerStash = World.GetStash<TimerComponent>();
            _survivalModeStash = World.GetStash<SurvivalModeComponent>();
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
            
            _unitSpawnRequest.Publish(new UnitSpawnRequest
            {
                UnitType = _settings.Allies[0],
                Position = Vector3.zero,
                LookDirection = Vector3.back,
                TeamId = 0,
                HasAI = false,
                IsPrioritizedTarget = true,
                Priority = 100
            });
        }

        private void SwitchToNextHero() => ChangeUnitTypes(1);
        private void SwitchToPreviousHero() => ChangeUnitTypes(-1);

        private void StartGame()
        {
            foreach (var entity in _survivalModeFilter)
                _timerStash.Set(entity);
            
            foreach (var entity in _gameCameraFilter)
            {
                ref var cameraComponent = ref _gameCameraStash.Get(entity);
                cameraComponent.Value.SetFightCamera();
            }
            
            _menuUIContainer.SetActivity(false);
            _gameplayUIContainer.SetActivity(true);
        }

        private void ChangeUnitTypes(int change)
        {
            foreach (var modeEntity in _survivalModeFilter)
            {
                ref var survivalModeComponent = ref _survivalModeStash.Get(modeEntity);
                var id = survivalModeComponent.HeroId + change;

                if (id >= _settings.Allies.Count)
                {
                    id -= _settings.Allies.Count;
                }
                else if (id < 0)
                {
                    id = _settings.Allies.Count - 1;
                }
                
                survivalModeComponent.HeroId = id;

                foreach (var unitEntity in _playerUnitFilter)
                {
                    var type = _settings.Allies[survivalModeComponent.HeroId];

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
                        Priority = priorityComponent.Value,
                        IsPrioritizedTarget = true
                    });

                    Object.Destroy(unitComponent.Value.gameObject);
                    World.RemoveEntity(unitEntity);
                }
            }
        }
    }
}