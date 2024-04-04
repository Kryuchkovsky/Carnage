using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Effects.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Attack.Requests;
using _Logic.Gameplay.Units.Components;
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
        private Request<AttackRequest> _attackRequest;
        private Event<AttackStartEvent> _attackStartEvent;

        public override void OnAwake()
        {
            _attackRequest = World.GetRequest<AttackRequest>();
            _attackStartEvent = World.GetEvent<AttackStartEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var ent in _attackStartEvent.publishedChanges)
            {
                if (ent.AttackingEntity.IsNullOrDisposed() || ent.AttackedEntity.IsNullOrDisposed() || !ent.AttackingEntity.Has<AttackComponent>()) continue;

                ref var unitComponent = ref ent.AttackingEntity.GetComponent<UnitComponent>(out var hasUnitComponent);
                ref var splitComponent = ref ent.AttackingEntity.GetComponent<SplitAttackComponent>(out var hasSplitComponent);
                ref var statsComponent = ref ent.AttackingEntity.GetComponent<StatsComponent>(out var hasStatsComponent);
                ref var teamDataComponent = ref ent.AttackingEntity.GetComponent<TeamComponent>(out var hasTeamDataComponent);

                if (!hasUnitComponent && !hasSplitComponent || !hasStatsComponent || !hasTeamDataComponent || splitComponent.AdditionalTargets <= 0) continue;

                var range = statsComponent.Value.GetCurrentValue(StatType.AttackRange);
                var ownerEntity = ent.AttackingEntity;
                ref var effectComponent = ref ent.AttackingEntity.GetComponent<EffectComponent>(out var hasEffectComponent);
                
                if (hasEffectComponent)
                {
                    ref var ownerComponent = ref ent.AttackingEntity.GetComponent<OwnerComponent>(out var hasOwnerComponent);
                    
                    if (!effectComponent.ModifiersAreInfluencing || !hasOwnerComponent || ownerComponent.Entity.IsNullOrDisposed()) continue;
                    
                    ownerEntity = ownerComponent.Entity;
                }

                var position = unitComponent.Value.transform.position;
                var attackPosition = unitComponent.Value.Model.AttackPoint.position;
                var mask = 1 << teamDataComponent.EnemiesLayer;
                var colliderNumber = Physics.OverlapSphereNonAlloc(position, range, _colliders, mask);
                var numberOfFoundedTargets = 0;
                
                for (int i = 0; i < colliderNumber && numberOfFoundedTargets < splitComponent.AdditionalTargets; i++)
                {
                    if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) && !linkedCollider.Entity.IsNullOrDisposed() && 
                        linkedCollider.Entity != ownerEntity && linkedCollider.Entity != ent.AttackedEntity)
                    {
                        _attackRequest.Publish(new AttackRequest
                        {
                            AttackerEntity = ownerEntity,
                            TargetEntity = linkedCollider.Entity,
                            AttackPosition = attackPosition
                        }, true);
                        
                        numberOfFoundedTargets++;
                    }
                }
            }
        }
    }
}