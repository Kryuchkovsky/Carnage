using System.Collections.Generic;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Patterns;
using _Logic.Gameplay.Units.Projectiles.Components;
using _Logic.Gameplay.Units.Projectiles.Events;
using _Logic.Gameplay.Units.Projectiles.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Projectiles.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ProjectileCreationRequestProcessingSystem : AbstractUpdateSystem
    {
        private Dictionary<ProjectileType, ObjectPool<ProjectileProvider>> _projectilePools;
        private Request<ProjectileCreationRequest> _projectileCreationRequest;
        private Event<ProjectileFlightEndEvent> _projectileFlightEndEvent;
        
        [Inject]
        private ProjectilesCatalog _projectilesCatalog;

        public override void OnAwake()
        {
            _projectilePools = new Dictionary<ProjectileType, ObjectPool<ProjectileProvider>>();
            _projectileCreationRequest = World.GetRequest<ProjectileCreationRequest>();
            _projectileFlightEndEvent = World.GetEvent<ProjectileFlightEndEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _projectileCreationRequest.Consume())
            {
                if (request.OwnerEntity.IsNullOrDisposed() || request.Type == ProjectileType.None || 
                    (request.TargetEntity.IsNullOrDisposed() && request.TargetPosition == Vector3.zero)) continue;

                var type = request.Type;
                var data = _projectilesCatalog.GetData((int)type);
                
                if (!_projectilePools.ContainsKey(type))
                {
                    _projectilePools.Add(type, new ObjectPool<ProjectileProvider>(data.Provider));
                }

                var projectile = _projectilePools[type].Take();
                projectile.transform.position = request.InitialPosition;
                
                projectile.Entity.SetComponent(new ProjectileDataComponent
                {
                    Value = data
                });
                projectile.Entity.SetComponent(new OwnerComponent
                {
                    Entity = request.OwnerEntity
                });
                
                if (request.TargetEntity.IsNullOrDisposed())
                {
                    projectile.Entity.SetComponent(new DestinationComponent
                    {
                        Value = request.TargetPosition
                    });
                }
                else
                {
                    projectile.Entity.SetComponent(new TargetComponent
                    {
                        Entity = request.TargetEntity
                    });
                }
            }
            
            foreach (var @event in _projectileFlightEndEvent.publishedChanges)
            {
                ref var projectileComponent = ref @event.ProjectileEntity.GetComponent<ProjectileComponent>();
                ref var projectileDataComponent = ref @event.ProjectileEntity.GetComponent<ProjectileDataComponent>();
                _projectilePools[projectileDataComponent.Value.Type].Return(projectileComponent.Value);
            }
        }
    }
}