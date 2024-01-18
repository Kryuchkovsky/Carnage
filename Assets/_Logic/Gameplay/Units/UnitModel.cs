using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitModel : MonoBehaviour
    {
        public virtual void PlayAttackAnimation()
        {
        }

        public virtual void SetMovementSpeed(float value)
        {
        }
    }
}