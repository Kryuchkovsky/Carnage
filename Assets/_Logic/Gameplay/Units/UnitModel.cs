using UnityEngine;
using UnityEngine.AI;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitModel : MonoBehaviour
    {
        [field: SerializeField] private Renderer Renderer;
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public Transform AttackPoint { get; private set; }
        [field: SerializeField] public EffectType HitEffectType { get; private set; }
        [field: SerializeField] public NavMeshObstacleShape NavMeshObstacleShape { get; private set; } = NavMeshObstacleShape.Capsule;
        [field: SerializeField] public int NavMeshAgentTypeId { get; private set; }
        
        public Bounds Bounds => Renderer.bounds;

        private void Awake()
        {
            if (!Renderer)
            {
                Renderer = GetComponentInChildren<Renderer>();
            }

            if (!Collider)
            {
                Collider = GetComponentInChildren<Collider>();
            }
        }

        public virtual void PlayAttackAnimation()
        {
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

        public virtual void LookAtPoint(Vector3 point)
        {
        }
    }
}