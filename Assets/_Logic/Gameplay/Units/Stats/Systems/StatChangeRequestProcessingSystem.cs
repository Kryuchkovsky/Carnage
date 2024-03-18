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
    public class StatChangeRequestProcessingSystem : AbstractUpdateSystem
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
                if (request.Entity.IsNullOrDisposed()) continue;
                
                ref var statsComponent = ref request.Entity.GetComponent<StatsComponent>(out var hasStatsComponent);
                
                if (hasStatsComponent && statsComponent.Value.TryGetValue(request.Type, out var stat))
                {
                    stat.AddModifier(request.Modifier);
                    
                    var statsPanelComponent = request.Entity.GetComponent<StatsPanelComponent>(out var hasStatsPanelComponent);
                    
                    if (hasStatsPanelComponent)
                    {
                        statsPanelComponent.Value.SetStat(request.Type, stat.CurrentValue);
                    }
                }
            }
        }
    }
}