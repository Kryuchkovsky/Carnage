using _Logic.Extensions.Configs;
using _Logic.Gameplay.Projectiles.Components;
using UnityEngine;

namespace _Logic.Gameplay.Projectiles
{
    [CreateAssetMenu(menuName = "Create ProjectileData", fileName = "ProjectileData")]
    public class ProjectileData : Data<ProjectileType>
    {
        [field: SerializeField] public ProjectileProvider Provider { get; private set; }

        [field: SerializeField]
        public FlightParametersComponent Parameters { get; private set; } = new()
        {
            Speed = 20
        };
    }
}