using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Spawn.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using Random = UnityEngine.Random;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class SpawnAbilityHandlingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<TimerComponent> _timerStash;
        private Stash<OwnerComponent> _ownerStash;
        private Stash<SpawnAbilityComponent> _spawnAbilityStash;
        private Stash<TeamComponent> _teamStash;
        private Stash<TransformComponent> _transformStash;
        private Request<UnitSpawnRequest> _unitSpawnRequest;

        public override void OnAwake()
        {
            _filter = World.Filter.With<SpawnAbilityComponent>().With<OwnerComponent>().With<TimerComponent>().Build();
            _timerStash = World.GetStash<TimerComponent>();
            _ownerStash = World.GetStash<OwnerComponent>();
            _spawnAbilityStash = World.GetStash<SpawnAbilityComponent>();
            _teamStash = World.GetStash<TeamComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var timerComponent = ref _timerStash.Get(entity);
                ref var ownerComponent = ref _ownerStash.Get(entity);

                if (timerComponent.Value > 0 || World.IsDisposed(ownerComponent.Entity) || 
                    !_teamStash.Has(ownerComponent.Entity) || !_transformStash.Has(ownerComponent.Entity)) 
                    continue;
                
                ref var spawnAbilityComponent = ref _spawnAbilityStash.Get(entity);

                var allUnitTypes = spawnAbilityComponent.UnitTypes;
                var unitType = allUnitTypes[Random.Range(0, allUnitTypes.Count)];

                ref var ownerTeamComponent = ref _teamStash.Get(ownerComponent.Entity);
                ref var ownerTransformComponent = ref _transformStash.Get(ownerComponent.Entity);
                    
                _unitSpawnRequest.Publish(new UnitSpawnRequest
                {
                    UnitType = unitType,
                    Position = ownerTransformComponent.Value.transform.position,
                    TeamId = ownerTeamComponent.Id,
                    HasAI = true
                });
                _timerStash.Set(entity, new TimerComponent
                {
                    Value = spawnAbilityComponent.Interval
                });
            }
        }
    }
}