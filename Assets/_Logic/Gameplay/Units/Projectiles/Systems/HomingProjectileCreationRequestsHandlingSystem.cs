using System.Collections.Generic;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Extensions.Patterns;
using _Logic.Gameplay.Units.Projectiles.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Projectiles.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HomingProjectileCreationRequestsHandlingSystem : AbstractUpdateSystem
    {
        private Dictionary<ProjectileType, ObjectPool<ProjectileProvider>> _projectilePools;
        private Request<HomingProjectileCreationRequest> _request;
        private ProjectilesCatalog _projectilesCatalog;

        public override void OnAwake()
        {
            _projectilePools = new Dictionary<ProjectileType, ObjectPool<ProjectileProvider>>();
            _request = World.GetRequest<HomingProjectileCreationRequest>();
            _projectilesCatalog = ConfigManager.Instance.GetConfig<ProjectilesCatalog>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (request.Target == null) continue;
                
                if (!_projectilePools.ContainsKey(request.Data.Type))
                {
                    _projectilePools.Add(request.Data.Type, new ObjectPool<ProjectileProvider>(
                        _projectilesCatalog.GetData((int)request.Data.Type).Provider));
                }

                var projectile = _projectilePools[request.Data.Type].Take();
                projectile.transform.position = request.InitialPosition;
                projectile.Entity.SetComponent(new ProjectileDataComponent
                {
                    Value = request.Data
                });
                projectile.Entity.SetComponent(new FollowingTransformComponent
                {
                    Transform = request.Target,
                    Offset = request.Offset
                });
                projectile.Entity.SetComponent(new DestinationComponent
                {
                    Value = request.Target.position + request.Offset
                });
                projectile.FlightEnded += request.Callback;
                projectile.FlightEnded += _projectilePools[request.Data.Type].Return;
            }
        }
    }
}