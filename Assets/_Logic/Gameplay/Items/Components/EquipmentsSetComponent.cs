using System.Collections.Generic;
using _Logic.Gameplay.Equipment;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Items.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EquipmentsSetComponent : IComponent
    {
        public Dictionary<SlotType, EquipmentData> Dictionary;
    }
}