using _Logic.Gameplay.Equipment.Weapon;
using UnityEngine;

namespace _Logic.Gameplay.Equipment
{
    public class EquipmentSet
    {
        [field: SerializeField] public WeaponData Weapon { get; private set; }
    }
}