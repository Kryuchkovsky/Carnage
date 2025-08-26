using System;
using _Logic.Core;
using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitModel : MonoBehaviour
    {
        [field: SerializeField] public LinkedCollider LinkedCollider { get; private set; }
        [field: SerializeField] public Renderer Renderer { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public Transform AttackPoint { get; private set; }
        [field: SerializeField] public NavMeshObstacleShape NavMeshObstacleShape { get; private set; } = NavMeshObstacleShape.Capsule;
        [field: SerializeField] public int NavMeshAgentTypeId { get; private set; }

        public Bounds Bounds => Renderer.bounds;
        public int Id { get; private set; }

        public void Initialize(int id)
        {
            Id = id;
            
            if (!Renderer)
            {
                Renderer = GetComponentInChildren<Renderer>();
            }

            if (!Collider)
            {
                Collider = GetComponentInChildren<Collider>();
            }
        }

        public virtual void PlayAttackAnimation(float attackSpeed = 1, Action callback = null)
        {
            callback?.Invoke();
        }
        
        public virtual void PlayHitAnimation()
        {
        }
        
        public virtual void PlayDeathAnimation()
        {
        }

        public virtual void SetMovementSpeed(float value)
        {
        }

        public virtual void LookAtPoint(Vector3 point, float rotationSpeed)
        {
        }   
        
        public virtual void Reset()
        {
        }
    }
}