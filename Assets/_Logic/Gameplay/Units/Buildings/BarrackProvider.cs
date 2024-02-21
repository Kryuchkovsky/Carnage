using _Logic.Core.Components;
using _Logic.Gameplay.Units.Buildings.Components;
using _Logic.Gameplay.Units.Team;
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
            
            World.Default.GetRequest<TeamDataSettingRequest>().Publish(new TeamDataSettingRequest
            {
                Entity = Entity,
                TeamId = _teamId
            }, true);
        }
    }
}