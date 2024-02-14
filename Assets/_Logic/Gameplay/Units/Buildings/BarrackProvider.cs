using _Logic.Core.Components;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.Buildings.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Buildings
{
    public class BarrackProvider : UnitProvider
    {
        [SerializeField] private int _teamId;
        
        [field: SerializeField] public Transform SpawnPoint { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.AddComponent<TimerComponent>();
            Entity.SetComponent(new BarrackComponent
            {
                Value = this
            });
            Entity.SetComponent(new TeamIdComponent
            {
                Value = _teamId
            });
        }
    }
}