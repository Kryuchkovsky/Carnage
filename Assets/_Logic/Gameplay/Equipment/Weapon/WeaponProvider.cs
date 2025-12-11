using UnityEngine;

namespace _Logic.Gameplay.Equipment.Weapon
{
    public class WeaponProvider : EquipmentProvider
    {
        [SerializeField] private ParticleSystem _attackEffect;
        
        [field: SerializeField] public Transform AttackPoint { get; private set; }

        public void OnAttack()
        {
            if (_attackEffect) 
                _attackEffect.Play();
        }
    }
}