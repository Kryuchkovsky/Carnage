using System.Collections.Generic;

namespace _Logic.Gameplay.Items.Equipment
{
    public static class EquipmentTypeExtensions
    {
        private static class SlotCache
        {
            private static readonly Dictionary<EquipmentType, SlotType> _cache;
            private static readonly Dictionary<EquipmentType, List<SlotType>> _cache1;
        
            static SlotCache()
            {
                _cache = new Dictionary<EquipmentType, SlotType>
                {
                    { EquipmentType.None, SlotType.None },
                    { EquipmentType.OneHandedSword, SlotType.LeftHand | SlotType.RightHand },
                    { EquipmentType.OneHandedSpear, SlotType.LeftHand | SlotType.RightHand },
                    { EquipmentType.OneHandedAxe, SlotType.LeftHand | SlotType.RightHand },
                    { EquipmentType.OneHandedMace, SlotType.LeftHand | SlotType.RightHand },
                    { EquipmentType.TwoHandedSword, SlotType.LeftHand & SlotType.RightHand },
                    { EquipmentType.TwoHandedSpear, SlotType.LeftHand & SlotType.RightHand },
                    { EquipmentType.TwoHandedAxe, SlotType.LeftHand & SlotType.RightHand },
                    { EquipmentType.TwoHandedMace, SlotType.LeftHand & SlotType.RightHand },
                    { EquipmentType.Bow, SlotType.LeftHand & SlotType.RightHand },
                    { EquipmentType.Rifle, SlotType.LeftHand & SlotType.RightHand }
                };
                
                _cache1 = new Dictionary<EquipmentType, List<SlotType>>
                {
                    { EquipmentType.None, new List<SlotType> {SlotType.None}},
                    { EquipmentType.OneHandedSword, new List<SlotType> {SlotType.LeftHand, SlotType.RightHand}},
                    { EquipmentType.OneHandedSpear, new List<SlotType> {SlotType.LeftHand, SlotType.RightHand}},
                    { EquipmentType.OneHandedAxe, new List<SlotType> {SlotType.LeftHand, SlotType.RightHand}},
                    { EquipmentType.OneHandedMace, new List<SlotType> {SlotType.LeftHand, SlotType.RightHand}},
                    { EquipmentType.TwoHandedSword, new List<SlotType> {SlotType.LeftHand, SlotType.RightHand}},
                };
            }
        
            public static bool TryGetSlot(EquipmentType type, out SlotType slotType) => 
                _cache.TryGetValue(type, out slotType);
        }

        public static SlotType GetEquipmentSlot(this EquipmentType type)
        {
            return SlotCache.TryGetSlot(type, out var slot) 
                ? slot 
                : SlotType.None;
        }
    }
}