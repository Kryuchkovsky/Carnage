using _GameLogic.Extensions;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.SurvivalMode.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Spawn;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemiesSpawnRequestSendingSystem : AbstractUpdateSystem
    {
        private FilterBuilder _survivalModeFilter;
        private FilterBuilder _playerFilter;
        private SurvivalModeSettings _settings;

        public override void OnAwake()
        {
            _survivalModeFilter = World.Filter.With<SurvivalModeComponent>().With<TimerComponent>();
            _playerFilter = World.Filter.With<UnitComponent>().With<TransformComponent>().Without<AIComponent>();
            _settings = ConfigManager.GetConfig<SurvivalModeSettings>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _survivalModeFilter.Build())
            {
                if (entity.GetComponent<TimerComponent>().Value > 0) continue;

                var playerEntity = _playerFilter.Build().FirstOrDefault();

                if (playerEntity.IsNullOrDisposed()) return;

                var position = playerEntity.GetComponent<TransformComponent>().Value.position;
                position += ExtraMethods.GetRandomDirectionXZ() * 50;
                var allUnitTypes = _settings.Units;
                var unitType = allUnitTypes[Random.Range(0, allUnitTypes.Count)];

                World.GetRequest<UnitSpawnRequest>().Publish(new UnitSpawnRequest
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