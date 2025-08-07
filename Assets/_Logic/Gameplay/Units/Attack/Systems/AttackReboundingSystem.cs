using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Effects.Components;
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
    public sealed class AttackReboundingSystem : AbstractUpdateSystem
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
                if (ent.AttackerEntity.IsNullOrDisposed() || ent.TargetEntity.IsNullOrDisposed() || !ent.AttackerEntity.Has<ReboundAttackComponent>() ||
                    !ent.AttackerEntity.Has<AttackComponent>() || !ent.AttackerEntity.Has<StatsComponent>() || !ent.AttackerEntity.Has<TeamComponent>()) 
                    continue;

                var ownerEntity = ent.AttackerEntity;
                var isEffect = ent.AttackerEntity.Has<EffectComponent>();
                ref var reboundComponent = ref ent.AttackerEntity.GetComponent<ReboundAttackComponent>();
                ref var attackComponent = ref ent.AttackerEntity.GetComponent<AttackComponent>();
                ref var statsComponent = ref ent.AttackerEntity.GetComponent<StatsComponent>();
                ref var teamDataComponent = ref ent.AttackerEntity.GetComponent<TeamComponent>();

                if (isEffect)
                {
                    ref var ownerComponent = ref ent.AttackerEntity.GetComponent<OwnerComponent>(out var hasOwnerComponent);
                    reboundComponent.Rebounds -= 1;

                    if (hasOwnerComponent && !ownerComponent.Entity.IsNullOrDisposed() && reboundComponent.Rebounds > 0)
                    {
                        ownerEntity = ownerComponent.Entity;
                    }
                    else
                    {
                        ent.AttackerEntity.Dispose();
                        continue;
                    }
                }

                ref var attackedUnitTransformComponent = ref ent.TargetEntity.GetComponent<TransformComponent>();
                ref var attackedUnitBoundsComponent = ref ent.TargetEntity.GetComponent<BoundsComponent>(out var attackedUnitHasBoundsComponent);
                var attackedUnitPosition = attackedUnitTransformComponent.Value.position;

                if (attackedUnitHasBoundsComponent)
                {
                    attackedUnitPosition += Vector3.up * attackedUnitBoundsComponent.Value.extents.y;
                }

                var range = statsComponent.Value.GetCurrentValue(StatType.AttackRange);
                var mask = 1 << teamDataComponent.EnemiesLayer;
                var colliderNumber = Physics.OverlapSphereNonAlloc(attackedUnitPosition, range, _colliders, mask);
                var targetWasFound = false;

                for (int i = 0; i < colliderNumber; i++)
                {
                    if (_colliders[i].TryGetComponent<LinkedCollider>(out var linkedCollider) && !linkedCollider.Entity.IsNullOrDisposed() &&
                        linkedCollider.Entity != ownerEntity && linkedCollider.Entity != ent.TargetEntity)
                    {
                        targetWasFound = true;
                        var attackerEntity = ent.AttackerEntity;

                        if (!isEffect)
                        {
                            attackerEntity = CreateEffectEntity(ent.AttackerEntity, attackComponent, statsComponent, teamDataComponent, reboundComponent.Rebounds);
                        }

                        _attackRequest.Publish(new AttackRequest
                        {
                            AttackerEntity = attackerEntity,
                            TargetEntity = linkedCollider.Entity,
                            AttackPosition = attackedUnitPosition
                        }, true);

                        break;
                    }
                }

                if (isEffect && !targetWasFound)
                {
                    ent.AttackerEntity.Dispose();
                }
            }
        }

        private Entity CreateEffectEntity(Entity ownerEntity, AttackComponent attackComponent, StatsComponent statsComponent, TeamComponent teamComponent, int rebounds)
        {
            var effectEntity = World.CreateEntity();
            effectEntity.SetComponent(new EffectComponent());
            effectEntity.SetComponent(attackComponent);
            effectEntity.SetComponent(statsComponent);
            effectEntity.SetComponent(teamComponent);
            effectEntity.SetComponent(new ReboundAttackComponent
            {
                Rebounds = rebounds
            });
            effectEntity.SetComponent(new OwnerComponent
            {
                Entity = ownerEntity
            });
            
            return effectEntity;
        }
    }
}