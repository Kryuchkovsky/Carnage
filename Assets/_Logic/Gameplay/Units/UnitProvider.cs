using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI;
using _Logic.Gameplay.Units.Attack.Components;
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
        [SerializeField, CanBeNull] protected NavMeshObstacle _navMeshObstacle;
        
        [SerializeField] private SpriteRenderer _markerSprite;
        [SerializeField, Range(0, 60)] private float _corpseExistenceTime = 3;
        
        [SerializeField, Range(0, 100)] private int _priority;
        [SerializeField] private bool _isPrioritizedTarget;
        
        [field: SerializeField, CanBeNull] public UnitModel Model { get; protected set; }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            gameObject.layer = 6;
            
            Entity.SetComponent(new UnitComponent
            {
                Value = this
            });

            if (Model)
            {
                Entity.SetComponent(new BoundsComponent
                {
                    Value = Model.Bounds
                });
            }
            
            if (_isPrioritizedTarget)
            {
                Entity.SetComponent(new PriorityComponent
                {
                    Value = _priority
                });
            }

            if (_collider)
            {
                Entity.SetComponent(new ColliderComponent
                {
                    Value = _collider
                });
            }
            
            if (_rigidbody)
            {
                Entity.SetComponent(new RigidbodyComponent()
                {
                    Value = _rigidbody
                });
            }
            
            if (_navMeshAgent)
            {
                Entity.SetComponent(new NavMeshAgentComponent
                {
                    Value = _navMeshAgent
                });
            }

            if (_navMeshObstacle)
            {
                Entity.SetComponent(new NavMeshObstacleComponent
                {
                    Value = _navMeshObstacle
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
            
            Entity.SetComponent(new BoundsComponent
            {
                Value = model.Bounds
            });
        }

        public virtual void SetColor(Color color)
        {
            _markerSprite.color = color;
        }

        public virtual void OnAttack()
        {
            Model?.PlayAttackAnimation();
        }

        public virtual void OnMove(float speed)
        {
            Model?.SetMovementSpeed(speed);
        }

        public virtual void OnDamage()
        {
            Model?.PlayHitAnimation();
        }

        public virtual void OnDie()
        {
            gameObject.layer = 7;
            Model?.PlayDeathAnimation();
            
            if (_navMeshAgent)
            {
                _navMeshAgent.enabled = false;
            }

            if (_navMeshObstacle)
            {
                _navMeshObstacle.enabled = false;
            }
            
            Destroy(gameObject, _corpseExistenceTime);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!Entity.IsNullOrDisposed() && Entity.Has<AttackComponent>())
            {
                var data = Entity.GetComponent<AttackComponent>().CurrentData;
                
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, data.Range);

                var searchRange = data.Range * ConfigsManager.GetConfig<AISettings>().TargetSearchRangeToAttackRangeRatio;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, searchRange);
            } 
        }
    }
}