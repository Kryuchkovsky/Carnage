using UnityEngine;

namespace _Logic.Gameplay.Items.Equipment.Weapon
{
    public class WeaponProvider : EquipmentProvider
    {
        [SerializeField] private ParticleSystem _attackEffect;

        public void OnAttack()
        {
            if (_attackEffect) 
                _attackEffect.Play();
        }
    }
}