using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitModel : MonoBehaviour
    {
        [field: SerializeField] public string HitEffectId { get; private set; } = "blood_explosion";
        
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