using _Logic.Core;
using _Logic.Gameplay.Units.Spawn.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Spawn
{
    public class UnitBuilderProvider : GameObjectProvider<UnitBuilderComponent>
    {
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField, Range(-1, 7)] public int TeamId { get; private set; }
        [field: SerializeField] public int Priority { get; private set; }
        [field: SerializeField] public bool IsPrioritizedTarget { get; private set; }
        [field: SerializeField] public bool HasAI { get; private set; } = true;

        
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new UnitBuilderComponent
            {
                Value = this
            });
        }
    }
}