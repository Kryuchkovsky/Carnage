using System;
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
    public sealed class HomingProjectileCreationRequestsHandlingSystem : AbstractSystem
    {
        private Dictionary<string, ObjectPool<ProjectileProvider>> _projectilePools;
        private Request<HomingProjectileCreationRequest> _request;
        private ProjectilesCatalog _projectilesCatalog;

        public override void OnAwake()
        {
            _projectilePools = new Dictionary<string, ObjectPool<ProjectileProvider>>();
            _request = World.GetRequest<HomingProjectileCreationRequest>();
            _projectilesCatalog = ConfigsManager.GetConfig<ProjectilesCatalog>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (!_projectilePools.ContainsKey(request.Data.Id))
                {
                    _projectilePools.Add(request.Data.Id, new ObjectPool<ProjectileProvider>(
                        _projectilesCatalog.GetProjectile(request.Data.Id)));
                }

                var projectile = _projectilePools[request.Data.Id].Take();
                projectile.transform.position = request.InitialPosition;
                projectile.Entity.SetComponent(new ProjectileDataComponent
                {
                    Value = request.Data
                });
                projectile.Entity.SetComponent(new FollowingTransformComponent
                {
                    Value = request.Target
                });
                projectile.Entity.SetComponent(new DestinationComponent
                {
                    EndValue = request.Target.position
                });
                projectile.FlightEnded += request.Callback;
                projectile.FlightEnded += _projectilePools[request.Data.Id].Return;
            }
        }
    }
}