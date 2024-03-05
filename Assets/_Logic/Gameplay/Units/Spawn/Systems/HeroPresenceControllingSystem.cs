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

        public override void OnAwake()
        {
            _playerUnitsFilter = World.Filter.With<UnitComponent>().Without<AIComponent>();
            World.GetRequest<UnitSpawnRequest>().Publish(new UnitSpawnRequest
            {
                UnitType = UnitType.HumanWarrior,
                Position = Vector3.zero,
                TeamId = 0,
                HasAI = false
            });
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}