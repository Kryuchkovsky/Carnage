using System.Collections.Generic;
using System.Linq;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units.Projectiles
{
    [CreateAssetMenu(menuName = "Create ProjectilesCatalog", fileName = "ProjectilesCatalog", order = 0)]
    public class ProjectilesCatalog : InitializableConfig
    {
        [SerializeField] private List<ProjectileProvider> _projectileProviders;
        
        private Dictionary<string, ProjectileProvider> _projectileProvidersDictionary;
        
        public override void Initialize()
        {
            base.Initialize();
            _projectileProvidersDictionary = _projectileProviders.ToDictionary(k => k.name, v => v);
        }

        public ProjectileProvider GetProjectile(string id) => _projectileProvidersDictionary[id];
    }
}