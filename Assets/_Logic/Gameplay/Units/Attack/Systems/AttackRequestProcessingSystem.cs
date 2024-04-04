using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Projectiles.Events;
using _Logic.Gameplay.Projectiles.Requests;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Attack.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackRequestProcessingSystem : AbstractUpdateSystem
    {
        private Event<ProjectileFlightEndEvent> _projectileFlightEndEvent;
        private Event<AttackEndEvent> _attackEndEvent;
        private Request<AttackRequest> _attackRequest;
        private Request<ProjectileCreationRequest> _projectileCreationRequest;

        public override void OnAwake()
        {
            _attackEndEvent = World.GetEvent<AttackEndEvent>();
            _attackRequest = World.GetRequest<AttackRequest>();
            _projectileFlightEndEvent = World.GetEvent<ProjectileFlightEndEvent>();
            _projectileCreationRequest = World.GetRequest<ProjectileCreationRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _attackRequest.Consume())
            {
                if (request.AttackerEntity.IsNullOrDisposed() || request.TargetEntity.IsNullOrDisposed() || 
                    !request.AttackerEntity.Has<AttackComponent>()) continue;
                
                ref var attackComponent = ref request.AttackerEntity.GetComponent<AttackComponent>();
                
                if (attackComponent.ProjectileType == ProjectileType.None)
                {
                    if (request.IsOriginal)
                    {
                        _attackEndEvent.NextFrame(new AttackEndEvent
                        {
                            AttackerEntity = request.AttackerEntity,
                            TargetEntity = request.TargetEntity,
                        });
                    }
                }
                else
                {
                    _projectileCreationRequest.Publish(new ProjectileCreationRequest
                    {
                        OwnerEntity = request.AttackerEntity,
                        TargetEntity = request.TargetEntity,
                        Type = attackComponent.ProjectileType,
                        InitialPosition = request.AttackPosition,
                        IsOriginal = request.IsOriginal
                    }, true);
                }
            }

            foreach (var ent in _projectileFlightEndEvent.publishedChanges)
            {
                if (ent.OwnerEntity.IsNullOrDisposed() || !ent.OwnerEntity.Has<AttackComponent>() || !ent.ProjectileEntity.Has<OriginComponent>()) continue;

                _attackEndEvent.NextFrame(new AttackEndEvent
                {
                    AttackerEntity = ent.OwnerEntity,
                    TargetEntity = ent.TargetEntity
                });
            }
        }
    }
}