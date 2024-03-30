using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Projectiles
{
    [CreateAssetMenu(menuName = "Create ProjectilesCatalog", fileName = "ProjectilesCatalog", order = 0)]
    public class ProjectilesCatalog : FunctionalConfig<ProjectileType, ProjectileData>
    {
    }
}