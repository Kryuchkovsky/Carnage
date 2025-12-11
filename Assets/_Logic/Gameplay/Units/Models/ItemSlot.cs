using _Logic.Gameplay.Equipment;
using _Logic.Gameplay.Items;
using UnityEngine;

namespace _Logic.Gameplay.Units.Models
{
    public class ItemSlot : MonoBehaviour
    {
        [field: SerializeField] public SlotType SlotType { get; private set; }

        private EquipmentProvider _equipment;
        
        public bool Get(out EquipmentProvider equipment)
        {
            equipment = _equipment;
            return equipment != null;
        }
        
        public void Set(EquipmentProvider equipment)
        {
            _equipment = equipment;

            var equipmentTransform = equipment.transform;
            equipmentTransform.parent = transform;
            equipmentTransform.localPosition = Vector3.zero;
            equipmentTransform.localRotation = Quaternion.identity;
        }
    }
}