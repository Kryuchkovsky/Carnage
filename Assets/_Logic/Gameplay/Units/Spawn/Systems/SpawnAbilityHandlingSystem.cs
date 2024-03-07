using _Logic.Core.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Spawn.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    public sealed class SpawnAbilityHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitComponent>().With<SpawnAbilityComponent>().With<TeamDataComponent>().With<TimerComponent>()
                .ForEach((Entity entity, ref UnitComponent unitComponent, ref SpawnAbilityComponent spawnAbilityComponent, 
                    ref TeamDataComponent teamDataComponent, ref TimerComponent timerComponent) =>
                {
                    if (timerComponent.Value <= 0)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            var allUnitTypes = spawnAbilityComponent.Data.Units;
                            var unitType = allUnitTypes[Random.Range(0, allUnitTypes.Count)];
                            
                            World.GetRequest<UnitSpawnRequest>().Publish(new UnitSpawnRequest
                            {
                                UnitType = unitType,
                                Position = unitComponent.Value.transform.position,
                                TeamId = teamDataComponent.Id,
                                HasAI = true
                            });
                            entity.SetComponent(new TimerComponent
                            {
                                Value = 300
                            });
                        }
                    }
                });
        }
    }
}