using _Logic.Core;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Team.Systems
{
    public class TeamDataSettingRequestsProcessingSystem : AbstractSystem
    {
        private Request<TeamDataSettingRequest> _request;
        private readonly int _maxTeamNumber = 2;
        
        public override void OnAwake()
        {
            _request = World.GetRequest<TeamDataSettingRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                var enemiesLayer = 0;
                
                for (int j = 0; j < _maxTeamNumber; j++)
                {
                    if (j == request.TeamId) continue;

                    var layer = LayerMask.NameToLayer($"Team{j}");
                    enemiesLayer |= layer;
                }

                var teamDataComponent = new TeamDataComponent
                {
                    Color = request.TeamId == 0 ? Color.blue : Color.red,
                    EnemiesLayer = enemiesLayer,
                    Id = request.TeamId
                };
                    
                request.Entity.SetComponent(teamDataComponent);
                var unitComponent = request.Entity.GetComponent<UnitComponent>(out var hasUnitComponent);
                    
                if (hasUnitComponent)
                {
                    var layer = LayerMask.NameToLayer($"Team{request.TeamId}");
                    unitComponent.Value.SetTeamData(teamDataComponent.Color, layer);
                }
            }
        }
    }
}