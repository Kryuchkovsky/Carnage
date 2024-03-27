using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Effects.Components;
using _Logic.Gameplay.Units.Projectiles.Requests;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackReboundingSystem : AbstractUpdateSystem
    {
        private readonly Collider[] _colliders = new Collider[64];
        private Request<ProjectileCreationRequest> _projectileCreationRequest;
        private Event<AttackEndEvent> _attackEndEvent;

        public override void OnAwake()
        {
            _projectileCreationRequest = World.GetRequest<ProjectileCreationRequest>();
            _attackEndEvent = World.GetEvent<AttackEndEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var commitmentEvent in _attackEndEvent.publishedChanges)
            {
                if (commitmentEvent.AttackingEntity.IsNullOrDisposed() || commitmentEvent.AttackedEntity.IsNullOrDisposed()) continue;

                var ownerEntity = commitmentEvent.AttackingEntity;
                
                ref var reboundComponent = ref commitmentEvent.AttackingEntity.GetComponent<ReboundAttackComponent>(out var hasReboundComponent);
                ref var attackComponent = ref commitmentEvent.AttackingEntity.GetComponent<AttackComponent>(out var hasAttackComponent);
                ref var statsComponent = ref commitmentEvent.AttackingEntity.GetComponent<StatsComponent>(out var hasStatsComponent);
                ref var teamDataComponent = ref commitmentEvent.AttackingEntity.GetComponent<TeamDataComponent>(out var hasTeamDataComponent);
                
                if (!hasReboundComponent || !hasAttackComponent || !hasStatsComponent || 
                    statsComponent.Value.TryGetCurrentValue(StatType.AttackRange, out var range) || !hasTeamDataComponent) continue;
                
                if (commitmentEvent.AttackingEntity.Has<EffectComponent>())
                {
                    ref var ownerComponent = ref commitmentEvent.AttackingEntity.GetComponent<OwnerComponent>(out var hasOwnerComponent);
                    
                    if (hasOwnerComponent && !ownerComponent.Entity.IsNullOrDisposed() && reboundComponent.Rebounds > 0)
                    {
                        ownerEntity = ownerComponent.Entity;
                    }
                    else continue;
                    
                    reboundComponent.Rebounds--;
                }
                
                ref var attackedUnitTransform = ref commitmentEvent.AttackedEntity.GetComponent<TransformComponent>();
                var attackedUnitPosition = attackedUnitTransform.Value.position;
                var mask = 1 << teamDataComponent.EnemiesLayer;
                var colliderNumber = Physics.OverlapSphereNonAlloc(attackedUnitPosition, range, _colliders, mask);
                var targetWasFound = false;
                
                for (int i = 0; i < colliderNumber; i++)
                {
                    if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) && !linkedCollider.Entity.IsNullOrDisposed() && 
                        linkedCollider.Entity != ownerEntity && linkedCollider.Entity != commitmentEvent.AttackedEntity)
                    {
                        targetWasFound = true;
                        
                        if (!commitmentEvent.AttackingEntity.Has<EffectComponent>())
                        {
                            ownerEntity = World.CreateEntity();
                            ownerEntity.SetComponent(new EffectComponent());
                            ownerEntity.SetComponent(reboundComponent);
                            ownerEntity.SetComponent(attackComponent);
                            ownerEntity.SetComponent(teamDataComponent);
                            ownerEntity.SetComponent(new OwnerComponent
                            {
                                Entity = commitmentEvent.AttackingEntity
                            });
                        }
                        
                        if (attackComponent.ProjectileType == ProjectileType.None)
                        {
                            //todo: create health change event
                        }
                        else
                        {
                            _projectileCreationRequest.Publish(new ProjectileCreationRequest
                            {
                                OwnerEntity = ownerEntity,
                                TargetEntity = linkedCollider.Entity,
                                Type = attackComponent.ProjectileType,
                                InitialPosition = attackedUnitPosition
                            });
                        }
                        
                        break;
                    }
                }

                if (commitmentEvent.AttackingEntity.Has<EffectComponent>() && !targetWasFound)
                {
                    commitmentEvent.AttackingEntity.Dispose();
                }
            }
        }
    }
}