using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units.Projectiles
{
    [CreateAssetMenu(menuName = "Create ProjectileData", fileName = "ProjectileData")]
    public class ProjectileData : Data<ProjectileType>
    {
        [field: SerializeField] public ProjectileProvider Provider { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField, Range(0, 1)] public float FlightRangeToHeightRatio { get; private set; }
    }
}