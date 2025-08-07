using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Effects.Components;
using _Logic.Gameplay.Projectiles.Requests;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Attack.Requests;
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
    public sealed class AttackFragmentationSystem : AbstractUpdateSystem
    {
        private readonly Collider[] _colliders = new Collider[64];
        private Request<AttackRequest> _attackRequest;
        private Event<AttackEndEvent> _attackEndEvent;

        public override void OnAwake()
        {
            _attackRequest = World.GetRequest<AttackRequest>();
            _attackEndEvent = World.GetEvent<AttackEndEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var ent in _attackEndEvent.publishedChanges)
            {
                if (ent.AttackerEntity.IsNullOrDisposed() || ent.TargetEntity.IsNullOrDisposed() ) continue;
                
                ref var fragmentationAttackComponent = ref ent.AttackerEntity.GetComponent<FragmentationAttackComponent>(out var hasFragmentationAttackComponent);
                ref var attackComponent = ref ent.AttackerEntity.GetComponent<AttackComponent>(out var hasAttackComponent);
                ref var statsComponent = ref ent.AttackerEntity.GetComponent<StatsComponent>(out var hasStatsComponent);
                ref var teamDataComponent = ref ent.AttackerEntity.GetComponent<TeamComponent>(out var hasTeamDataComponent);

                if (!hasFragmentationAttackComponent || !hasAttackComponent || !hasStatsComponent || !hasTeamDataComponent || fragmentationAttackComponent.Fragments <= 0) continue;

                var range = statsComponent.Value.GetCurrentValue(StatType.AttackRange);
                var ownerEntity = ent.AttackerEntity;
                ref var effectComponent = ref ent.AttackerEntity.GetComponent<EffectComponent>(out var hasEffectComponent);
                
                if (hasEffectComponent)
                {
                    ref var ownerComponent = ref ent.AttackerEntity.GetComponent<OwnerComponent>(out var hasOwnerComponent);
                    
                    if (!effectComponent.ModifiersAreInfluencing || !hasOwnerComponent || ownerComponent.Entity.IsNullOrDisposed()) 
                        continue;
                    
                    ownerEntity = ownerComponent.Entity;
                }
                
                ref var attackedUnitTransformComponent = ref ent.TargetEntity.GetComponent<TransformComponent>();
                ref var attackedUnitBoundsComponent = ref ent.TargetEntity.GetComponent<BoundsComponent>(out var attackedUnitHasBoundsComponent);
                var attackPosition = attackedUnitTransformComponent.Value.position;

                if (attackedUnitHasBoundsComponent)
                {
                    attackPosition += Vector3.up * attackedUnitBoundsComponent.Value.extents.y;
                }
                
                var mask = 1 << teamDataComponent.EnemiesLayer;
                var colliderNumber = Physics.OverlapSphereNonAlloc(attackPosition, range, _colliders, mask);
                var numberOfFoundedTargets = 0;
                
                for (int i = 0; i < colliderNumber && numberOfFoundedTargets < fragmentationAttackComponent.Fragments; i++)
                {
                    if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) && !linkedCollider.Entity.IsNullOrDisposed() && 
                        linkedCollider.Entity != ownerEntity && linkedCollider.Entity != ent.TargetEntity)
                    {
                        _attackRequest.Publish(new AttackRequest
                        {
                            AttackerEntity = ownerEntity,
                            TargetEntity = linkedCollider.Entity,
                            AttackPosition = attackPosition,
                            IsOriginal = true
                        }, true);
                        
                        numberOfFoundedTargets++;
                    }
                }
            }
        }
    }
}