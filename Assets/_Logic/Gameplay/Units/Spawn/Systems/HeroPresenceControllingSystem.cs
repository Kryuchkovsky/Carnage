using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.SurvivalMode;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class HeroPresenceControllingSystem : AbstractUpdateSystem
    {
        private FilterBuilder _playerUnitsFilter;
        private Request<UnitSpawnRequest> _unitSpawnRequest;
        private SurvivalModeSettings _survivalModeSettings;

        public override void OnAwake()
        {
            _playerUnitsFilter = World.Filter.With<UnitComponent>().Without<AIComponent>();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
            _survivalModeSettings = ConfigManager.Instance.GetConfig<SurvivalModeSettings>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_playerUnitsFilter.Build().IsEmpty())
            {
                _unitSpawnRequest.Publish(new UnitSpawnRequest
                {
                    UnitType = _survivalModeSettings.Allies[Random.Range(0, _survivalModeSettings.Allies.Count)],
                    Position = Vector3.zero,
                    TeamId = 0,
                    HasAI = false,
                    IsPrioritizedTarget = true,
                    Priority = 100
                });
            }
        }
    }
}