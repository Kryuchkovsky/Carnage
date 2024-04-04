using System;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.AI;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using JetBrains.Annotations;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Gameplay.Units
{
    public class UnitProvider : GameObjectProvider<UnitComponent>
    {
        [SerializeField, CanBeNull] protected Rigidbody _rigidbody;
        [SerializeField, CanBeNull] protected NavMeshAgent _navMeshAgent;
        [SerializeField, CanBeNull] protected NavMeshObstacle _navMeshObstacle;
        
        [SerializeField] private LinkedCollider _linkedCollider;
        [SerializeField] private SpriteRenderer _markerSprite;
        
        [SerializeField, Range(0, 100)] private int _priority;
        [SerializeField] private bool _isPrioritizedTarget;
        
        [field: SerializeField] public UnitModel Model { get; protected set; }
        
        protected override void Initialize()
        {
            base.Initialize();
            
            gameObject.layer = LayerMask.NameToLayer("Unit");
            
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
                
                Entity.SetComponent(new ColliderComponent
                {
                    Value = Model.Collider
                });
            }
            
            if (_isPrioritizedTarget)
            {
                Entity.SetComponent(new PriorityComponent
                {
                    Value = _priority
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

        public void SetModel(UnitModel model)
        {
            if (Model)
            {
                Destroy(Model.gameObject);
            }

            Model = model;
            model.transform.parent = transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.rotation = Quaternion.identity;
            
            Entity.SetComponent(new BoundsComponent
            {
                Value = model.Bounds
            });

            Entity.SetComponent(new ColliderComponent
            {
                Value = model.Collider
            });
            
            Entity.SetComponent(new ColliderComponent
            {
                Value = model.Collider
            });
            
            if (_navMeshAgent)
            {
                _navMeshAgent.agentTypeID = model.NavMeshAgentTypeId;
                _navMeshAgent.height = model.Bounds.size.y;
                _navMeshAgent.radius = model.Bounds.extents.z;
            }

            if (_navMeshObstacle)
            {
                _navMeshObstacle.shape = model.NavMeshObstacleShape;
                _navMeshObstacle.height = model.Bounds.size.y;
                _navMeshObstacle.radius = model.Bounds.extents.z;
                _navMeshObstacle.size = model.Bounds.size;
                _navMeshObstacle.center = model.Bounds.center - transform.position;
            }
        }

        public void SetTeamData(Color color, int teamLayer)
        {
            _linkedCollider?.Initiate(Entity, teamLayer);
            _markerSprite.color = color;
        }

        public void OnAttack(float attackSpeed = 1, Action callback = null)
        {
            Model?.PlayAttackAnimation(attackSpeed, callback);
        }

        public void OnMove(float speed)
        {
            Model?.SetMovementSpeed(speed);
        }

        public void OnDamage()
        {
            Model?.PlayHitAnimation();
        }

        public void OnDie(float delay = 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Corpse");
            Model?.PlayDeathAnimation();
            
            if (_navMeshAgent)
            {
                _navMeshAgent.enabled = false;
            }

            if (_navMeshObstacle)
            {
                _navMeshObstacle.enabled = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Entity.IsNullOrDisposed() && Entity.Has<AttackComponent>() && Entity.Has<StatsComponent>())
            {
                var stats = Entity.GetComponent<StatsComponent>().Value;
                var attackRange = stats.GetCurrentValue(StatType.AttackRange);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, attackRange);

                var searchRange = attackRange * ConfigManager.Instance.GetConfig<AISettings>().TargetSearchRangeToAttackRangeRatio;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, searchRange);
            } 
        }
#endif
    }
}