using System;
using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using JetBrains.Annotations;
using Pathfinding;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public class UnitProvider : GameObjectProvider<UnitComponent>
    {
        [SerializeField, CanBeNull] protected Rigidbody _rigidbody;
        
        [SerializeField, CanBeNull] protected AIPath _path;

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

                Entity.SetComponent(new RendererComponent
                {
                    Value = Model.Renderer
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
            
            if (_path)
            {
                _path.enabled = true;

                Entity.SetComponent(new PathfinderComponent()
                {
                    Value = _path
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
            model.transform.localRotation = Quaternion.identity;
            
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
            
            Entity.SetComponent(new RendererComponent
            {
                Value = Model.Renderer
            });
        }

        public void SetTeamData(Color color, int teamLayer)
        {
            Model?.LinkedCollider?.Initiate(Entity, teamLayer);
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

        public void OnDie()
        {
            if (Model)
            {
                Model.PlayDeathAnimation();
                Model.LinkedCollider.enabled = false;
                Model.gameObject.layer = LayerMask.NameToLayer("Corpse");
                Model.Renderer.material.color = Color.grey;
            }

            if (_path)
                _path.enabled = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Entity.IsNullOrDisposed() && Entity.Has<StatsComponent>())
            {
                var attackRange = Entity.GetComponent<StatsComponent>().Value.GetCurrentValue(StatType.AttackRange);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, attackRange);

                var searchRange = Entity.GetComponent<StatsComponent>().Value.GetCurrentValue(StatType.VisionRange);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, searchRange);
            } 
        }
#endif
    }
}