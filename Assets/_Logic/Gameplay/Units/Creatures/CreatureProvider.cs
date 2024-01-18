using _Logic.Core.Components;
using _Logic.Gameplay.Units.Creatures.Components;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Gameplay.Units.Creatures
{
    public class CreatureProvider : UnitProvider
    {
        [SerializeField, CanBeNull] private NavMeshAgent _navMeshAgent;

        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new CreatureComponent
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
        }

        public override void OnDie()
        {
            base.OnDie();
            
            if (_navMeshAgent != null)
            {
                _navMeshAgent.ResetPath();
            }
        }
    }
}