using _Logic.Core;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Health.Events;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Health.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PeriodicHealthChangesResetSystem : AbstractUpdateSystem
    {
        private Event<UnitDeathEvent> _unitDeathEvent;
        
        public override void OnAwake()
        {
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var ent in _unitDeathEvent.publishedChanges)
            {
                if (ent.CorpseEntity.IsNullOrDisposed() || !ent.CorpseEntity.Has<PeriodicHealthChangesComponent>()) continue;
                
                ref var periodicHealthChangesComponent = ref ent.CorpseEntity.GetComponent<PeriodicHealthChangesComponent>();
                periodicHealthChangesComponent.Value.Clear();
            }
        }
    }
}