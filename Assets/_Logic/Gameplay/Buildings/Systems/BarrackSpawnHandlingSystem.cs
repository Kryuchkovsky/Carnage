﻿using _Logic.Core.Components;
using _Logic.Gameplay.Buildings.Components;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Buildings.Systems
{
    public class BarrackSpawnHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<BarrackComponent>().With<TeamIdComponent>().With<TimerComponent>()
                .ForEach((Entity entity, ref BarrackComponent barrackComponent, ref TeamIdComponent teamId, ref TimerComponent timerComponent) =>
                {
                    if (timerComponent.Value <= 0)
                    {
                        World.GetRequest<UnitSpawnRequest>().Publish(new UnitSpawnRequest
                        {
                            Position = barrackComponent.Value.SpawnPoint.position,
                            TeamId = teamId.Value,
                            UnitId = "knight"
                        });
                        entity.SetComponent(new TimerComponent
                        {
                            Value = 1
                        });
                    }
                });
        }
    }
}