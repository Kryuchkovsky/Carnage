using _Logic.Core.Components;
using _Logic.Gameplay.Units.Spawn.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class SpawnAbilityHandlingSystem : QuerySystem
    {
        private Request<UnitSpawnRequest> _unitSpawnRequest;

        public override void OnAwake()
        {
            base.OnAwake();
            _unitSpawnRequest = World.GetRequest<UnitSpawnRequest>();
        }

        protected override void Configure()
        {
            CreateQuery()
                .With<SpawnAbilityComponent>().With<OwnerComponent>().With<TimerComponent>()
                .ForEach((Entity entity, ref SpawnAbilityComponent spawnAbilityComponent, ref OwnerComponent ownerComponent, ref TimerComponent timerComponent) =>
                {
                    if (timerComponent.Value > 0 || ownerComponent.Entity.IsNullOrDisposed() ||
                        !ownerComponent.Entity.Has<TeamDataComponent>() || !ownerComponent.Entity.Has<TransformComponent>()) return;

                    var allUnitTypes = spawnAbilityComponent.UnitTypes;
                    var unitType = allUnitTypes[Random.Range(0, allUnitTypes.Count)];

                    ref var ownerTeamDataComponent = ref ownerComponent.Entity.GetComponent<TeamDataComponent>();
                    ref var ownerTransformComponent = ref ownerComponent.Entity.GetComponent<TransformComponent>();
                    
                    _unitSpawnRequest.Publish(new UnitSpawnRequest
                    {
                        UnitType = unitType,
                        Position = ownerTransformComponent.Value.transform.position,
                        TeamId = ownerTeamDataComponent.Id,
                        HasAI = true
                    });
                    entity.SetComponent(new TimerComponent
                    {
                        Value = spawnAbilityComponent.Interval
                    });
                });
        }
    }
}