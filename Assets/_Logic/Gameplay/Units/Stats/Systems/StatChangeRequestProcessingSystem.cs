using _Logic.Core;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Stats.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Stats.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class StatModifierAdditionRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<StatChangeRequest> _request;
        
        public override void OnAwake()
        {
            _request = World.GetRequest<StatChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                var stats = request.Entity.GetComponent<StatsComponent>(out var hasStatsComponent).Value;
                var statType = request.Change.Type;

                if (hasStatsComponent && stats.TryGetValue(statType, out var stat))
                {
                    var statModifier = request.Change.IsPersist
                        ? new StatModifier(request.Change.OperationType, request.Change.Value)
                        : new StatModifier(request.Change.OperationType, request.Change.Value, request.Change.Duration);
                    
                    stat.AddModifier(statModifier);
                }
            }
        }
    }
}