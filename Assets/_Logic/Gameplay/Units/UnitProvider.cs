using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Components;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitProvider : GameObjectProvider<UnitComponent>
    {
        [SerializeField, CanBeNull] protected Collider _collider;
        [SerializeField, CanBeNull] protected Rigidbody _rigidbody;
        [SerializeField, CanBeNull] protected NavMeshAgent _navMeshAgent;
        
        public UnitModel Model { get; protected set; }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            gameObject.layer = 6;
            
            Entity.SetComponent(new UnitComponent
            {
                Value = this
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

        public virtual void SetModel(UnitModel model)
        {
            if (Model)
            {
                Destroy(Model);
            }

            Model = model;
            model.transform.parent = transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.rotation = Quaternion.identity;
        }

        public virtual void OnAttack()
        {
            Model.PlayAttackAnimation();
        }

        public virtual void OnMove(float speed)
        {
            Model.SetMovementSpeed(speed);
        }

        public virtual void OnDamage()
        {
            Model.PlayHitAnimation();
        }

        public virtual void OnDie()
        {
            gameObject.layer = 7;
            Model.PlayDeathAnimation();
            Destroy(gameObject, 3);
        }
    }
}