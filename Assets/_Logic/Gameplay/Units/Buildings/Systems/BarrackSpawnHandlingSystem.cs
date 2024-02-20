using _Logic.Core.Components;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Buildings.Components;
using _Logic.Gameplay.Units.Spawn;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Buildings.Systems
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
                        for (int i = 0; i < 10; i++)
                        {
                            World.GetRequest<UnitSpawnRequest>().Publish(new UnitSpawnRequest
                            {
                                Position = barrackComponent.Value.SpawnPoint.position,
                                TeamId = teamId.Value,
                                UnitId = teamId.Value == 0 ? "knight" : "warrior",
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