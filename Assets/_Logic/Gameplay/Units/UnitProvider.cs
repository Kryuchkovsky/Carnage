using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Components;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Gameplay.Units
{
    public class UnitProvider : ExtendedMonoProvider<UnitComponent>
    {
        [SerializeField] private Animator _animator;
        [SerializeField, CanBeNull] private NavMeshAgent _navMeshAgent;

        [SerializeField] private UnitDataComponent _dataComponent;

        private int _movementAnimationId;
        
        protected override void Initialize()
        {
            base.Initialize();

            _movementAnimationId = Animator.StringToHash("Speed_f");
            
            Entity.SetComponent(new UnitComponent
            {
                Value = this
            });
            
            if (_navMeshAgent != null)
            {
                Entity.SetComponent(new NavMeshAgentComponent
                {
                    Value = _navMeshAgent
                });
            }
            
            Entity.SetComponent(_dataComponent);
        }

        public void SetMovementSpeed(float value)
        {
            _animator.SetFloat(_movementAnimationId, value);
        }
    }
}