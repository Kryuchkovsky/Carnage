using _Logic.Gameplay.Units.Spawn.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnitBuilderHandlingSystem : QuerySystem
    {
        protected override void Configure()
        {
            CreateQuery()
                .With<UnitBuilderComponent>()
                .ForEach((Entity entity, ref UnitBuilderComponent unitBuilderComponent) =>
                {
                    var provider = unitBuilderComponent.Value;

                    World.GetRequest<UnitSpawnRequest>().Publish(new UnitSpawnRequest
                    {
                        UnitType = provider.UnitType,
                        Position = provider.transform.position,
                        TeamId = provider.TeamId,
                        IsPrioritizedTarget = provider.IsPrioritizedTarget,
                        Priority = provider.Priority,
                        HasAI = provider.HasAI
                    }, true);

                    World.RemoveEntity(provider.Entity);
                    Object.Destroy(provider.gameObject);
                });
        }
    }
}