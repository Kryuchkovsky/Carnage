using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI.Systems
{
    public sealed class UnitSightHandlingSystem : AbstractUpdateSystem
    {
        private Filter _filter;
        private Stash<UnitComponent> _unitStash;
        private Stash<TransformComponent> _transformStash;
        private Stash<AttackTargetComponent> _attackTargetStash;

        public override void OnAwake()
        {
            _filter = World.Filter.With<UnitComponent>().With<AttackComponent>().With<TransformComponent>()
                .With<AliveComponent>().Build();
            _unitStash = World.GetStash<UnitComponent>();
            _transformStash = World.GetStash<TransformComponent>();
            _attackTargetStash = World.GetStash<AttackTargetComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var unitComponent = ref _unitStash.Get(entity);
                ref var transformComponent = ref _transformStash.Get(entity);
                
                Vector3 sightPosition;
                ref var targetComponent = ref _attackTargetStash.Get(entity, out var hasTargetComponent);
                    
                if (hasTargetComponent && !World.IsDisposed(targetComponent.TargetEntity) &&
                    _transformStash.Has(targetComponent.TargetEntity) && targetComponent.IsInAttackRadius)
                {
                    sightPosition = _transformStash.Get(targetComponent.TargetEntity).Value.position;
                }
                else
                {
                    sightPosition = transformComponent.Value.position + transformComponent.Value.forward;
                }
                    
                unitComponent.Value.Model.LookAtPoint(sightPosition);            }
        }
    }
}