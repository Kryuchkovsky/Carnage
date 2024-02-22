using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitModel : MonoBehaviour
    {
        [SerializeField] private Renderer _meshRenderer;

        [field: SerializeField] public Transform AttackPoint { get; private set; }
        [field: SerializeField] public EffectType HitEffectType { get; private set; }

        public Bounds Bounds => _meshRenderer.bounds;

        private void Awake()
        {
            if (!_meshRenderer)
            {
                _meshRenderer = GetComponentInChildren<Renderer>();
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
    }
}