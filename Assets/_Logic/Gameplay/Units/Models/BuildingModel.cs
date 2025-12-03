using _Logic.Gameplay.Items;
using _Logic.Gameplay.Items.Equipment;

namespace _Logic.Gameplay.Units.Models
{
    public class BuildingModel : UnitModel
    {
        public override bool GetEquipment(SlotType slotType, out EquipmentProvider equipment)
        {
            equipment = null;
            return false;
        }

        public override void SetEquipment(SlotType slotType, EquipmentProvider equipment)
        {
        }
    }
}