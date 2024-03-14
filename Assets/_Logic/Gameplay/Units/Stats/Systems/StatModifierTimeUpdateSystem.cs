using _Logic.Core;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Stats.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class StatModifierTimeUpdateSystem : AbstractUpdateSystem
    {
        private FilterBuilder _statsFilter;
        
        public override void OnAwake()
        {
            _statsFilter = World.Filter.With<StatsComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _statsFilter.Build())
            {
                foreach (var stat in entity.GetComponent<StatsComponent>().Value.Values)
                {
                    stat.UpdateModifiers(deltaTime);
                }
            }
        }
    }
}