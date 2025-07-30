using _Logic.Core;
using _Logic.Gameplay.Units.Spawn.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Object = UnityEngine.Object;

namespace _Logic.Gameplay.Units.Spawn.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnitBuilderHandlingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitBuilderComponent> _unitBuilderStash;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitBuilderComponent>().Build();
            _unitBuilderStash = World.GetStash<UnitBuilderComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                var provider = _unitBuilderStash.Get(entity).Value;

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
            }
        }
    }
}