using _Logic.Gameplay.Items.Equipment.Weapon;
using UnityEngine;

namespace _Logic.Gameplay.Items.Equipment
{
    public class EquipmentSet
    {
        [field: SerializeField] public WeaponData Weapon { get; private set; }
    }
}