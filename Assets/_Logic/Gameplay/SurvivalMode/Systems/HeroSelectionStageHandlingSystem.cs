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
        private FilterBuilder _survivalModeFilter;
        private FilterBuilder _playerUnitFilter;
        private FilterBuilder _gameCameraFilter;
        private Request<UnitSpawnRequest> _unitSpawnRequest;

        [Inject] private MenuUIContainer _menuUIContainer;
        [Inject] private GameplayUIContainer _gameplayUIContainer;
        [Inject] private SurvivalModeSettings _settings;

        public override void OnAwake()
        {
            _survivalModeFilter = World.Filter.With<SurvivalModeComponent>();
            _playerUnitFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().With<TeamComponent>().With<PriorityComponent>().Without<AIComponent>();
            _gameCameraFilter = World.Filter.With<GameCameraComponent>();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _menuUIContainer.SetActivity(true);
            _gameplayUIContainer.SetActivity(false);
            _menuUIContainer.NextHeroButton.onClick.AddListener(SwitchToNextHero);
            _menuUIContainer.PreviousHeroButton.onClick.AddListener(SwitchToPreviousHero);
            _menuUIContainer.StartButton.onClick.AddListener(StartGame);

            foreach (var entity in _gameCameraFilter.Build())
            {
                ref var cameraComponent = ref entity.GetComponent<GameCameraComponent>();
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
            foreach (var entity in _survivalModeFilter.Build())
            {
                entity.SetComponent(new TimerComponent());
            }
            
            foreach (var entity in _gameCameraFilter.Build())
            {
                ref var cameraComponent = ref entity.GetComponent<GameCameraComponent>();
                cameraComponent.Value.SetFightCamera();
            }
            
            _menuUIContainer.SetActivity(false);
            _gameplayUIContainer.SetActivity(true);
        }

        private void ChangeUnitTypes(int change)
        {
            foreach (var modeEntity in _survivalModeFilter.Build())
            {
                ref var survivalModeComponent = ref modeEntity.GetComponent<SurvivalModeComponent>();
                survivalModeComponent.HeroId = (survivalModeComponent.HeroId + change) % _settings.Allies.Count;

                foreach (var unitEntity in _playerUnitFilter.Build())
                {
                    var type = _settings.Allies[survivalModeComponent.HeroId];

                    ref var unitComponent = ref unitEntity.GetComponent<UnitComponent>();
                    ref var transformComponent = ref unitEntity.GetComponent<TransformComponent>();
                    ref var teamComponent = ref unitEntity.GetComponent<TeamComponent>();
                    ref var priorityComponent = ref unitEntity.GetComponent<PriorityComponent>();
                    
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
                    unitEntity.Dispose();
                }
            }
        }
    }
}