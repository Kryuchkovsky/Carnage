using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitModel : MonoBehaviour
    {
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