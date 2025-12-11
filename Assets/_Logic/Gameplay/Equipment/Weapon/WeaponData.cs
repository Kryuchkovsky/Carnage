using _Logic.Gameplay.Projectiles;
using _Logic.Gameplay.Units.Health;
using UnityEngine;

namespace _Logic.Gameplay.Equipment.Weapon
{
    [CreateAssetMenu(menuName = nameof(WeaponData), fileName = nameof(WeaponData), order = 0)]
    public class WeaponData : EquipmentData
    {
        [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
        [field: SerializeField] public HealthChangeType AttackHealthChangeType { get; private set; }
        
        public override EquipmentCategory Category => EquipmentCategory.Weapon;
    }
}