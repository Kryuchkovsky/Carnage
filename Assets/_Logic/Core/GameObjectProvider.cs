using _Logic.Core.Components;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Core
{
    public abstract class GameObjectProvider<T> : MonoProvider<T> where T : struct, IComponent
    {
        [SerializeField, CanBeNull] protected Rigidbody _rigidbody;
        [SerializeField, CanBeNull] protected NavMeshAgent _navMeshAgent;

        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new TransformComponent
            {
                Value = transform
            });

            if (_rigidbody != null)
            {
                Entity.SetComponent(new RigidbodyComponent()
                {
                    Value = _rigidbody
                });
            }
            
            if (_navMeshAgent != null)
            {
                Entity.SetComponent(new NavMeshAgentComponent
                {
                    Value = _navMeshAgent
                });
            }
        }
    }
}