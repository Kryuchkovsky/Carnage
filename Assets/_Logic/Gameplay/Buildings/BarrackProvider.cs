using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Buildings.Components;
using _Logic.Gameplay.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Buildings
{
    public class BarrackProvider : ExtendedMonoProvider<BarrackComponent>
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
