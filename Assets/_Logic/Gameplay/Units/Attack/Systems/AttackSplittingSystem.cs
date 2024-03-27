using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Components;
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
    public sealed class AttackSplittingSystem : AbstractUpdateSystem
    {
        private readonly Collider[] _colliders = new Collider[64];
        private Request<ProjectileCreationRequest> _projectileCreationRequest;
        private Event<AttackStartEvent> _attackCommitmentEvent;

        public override void OnAwake()
        {
            _projectileCreationRequest = World.GetRequest<ProjectileCreationRequest>();
            _attackCommitmentEvent = World.GetEvent<AttackStartEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var attackStartEvent in _attackCommitmentEvent.publishedChanges)
            {
                if (attackStartEvent.AttackingEntity.IsNullOrDisposed() || attackStartEvent.AttackedEntity.IsNullOrDisposed()) continue;

                ref var unitComponent = ref attackStartEvent.AttackingEntity.GetComponent<UnitComponent>(out var hasUnitComponent);
                ref var splitComponent = ref attackStartEvent.AttackingEntity.GetComponent<SplitAttackComponent>(out var hasSplitComponent);
                ref var attackComponent = ref attackStartEvent.AttackingEntity.GetComponent<AttackComponent>(out var hasAttackComponent);
                ref var statsComponent = ref attackStartEvent.AttackingEntity.GetComponent<StatsComponent>(out var hasStatsComponent);
                ref var teamDataComponent = ref attackStartEvent.AttackingEntity.GetComponent<TeamDataComponent>(out var hasTeamDataComponent);

                if (!hasUnitComponent && !hasSplitComponent || !hasAttackComponent || !hasStatsComponent || 
                    !statsComponent.Value.TryGetCurrentValue(StatType.AttackRange, out var range)|| !hasTeamDataComponent || splitComponent.AdditionalTargets <= 0) continue;
                
                var ownerEntity = attackStartEvent.AttackingEntity;
                ref var effectComponent = ref attackStartEvent.AttackingEntity.GetComponent<EffectComponent>(out var hasEffectComponent);
                
                if (hasEffectComponent)
                {
                    ref var ownerComponent = ref attackStartEvent.AttackingEntity.GetComponent<OwnerComponent>(out var hasOwnerComponent);
                    
                    if (!effectComponent.ModifiersAreInfluencing || !hasOwnerComponent || ownerComponent.Entity.IsNullOrDisposed()) continue;
                    
                    ownerEntity = ownerComponent.Entity;
                }

                var position = unitComponent.Value.transform.position;
                var projectilePosition = unitComponent.Value.Model.AttackPoint.position;
                var mask = 1 << teamDataComponent.EnemiesLayer;
                var colliderNumber = Physics.OverlapSphereNonAlloc(position, range, _colliders, mask);
                var numberOfFoundedTargets = 0;
                
                for (int i = 0; i < colliderNumber && numberOfFoundedTargets < splitComponent.AdditionalTargets; i++)
                {
                    if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) && !linkedCollider.Entity.IsNullOrDisposed() && 
                        linkedCollider.Entity != ownerEntity && linkedCollider.Entity != attackStartEvent.AttackedEntity)
                    {
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
                                InitialPosition = projectilePosition
                            });
                        }
                        
                        numberOfFoundedTargets++;
                    }
                }
            }
        }
    }
}