using _Logic.Core;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class HeroPresenceControllingSystem : AbstractSystem
    {
        private FilterBuilder _playerUnitsFilter;
        private Request<UnitSpawnRequest> _request;

        public override void OnAwake()
        {
            _playerUnitsFilter = World.Filter.With<UnitComponent>().Without<AIComponent>();
            _request = World.GetRequest<UnitSpawnRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_playerUnitsFilter.Build().IsEmpty())
            {
                _request.Publish(new UnitSpawnRequest
                {
                    UnitType = UnitType.HumanMage,
                    Position = Vector3.zero,
                    TeamId = 0,
                    HasAI = false
                });
            }
        }
    }
}