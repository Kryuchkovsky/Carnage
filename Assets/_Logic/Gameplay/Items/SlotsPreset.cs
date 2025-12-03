using System.Collections.Generic;
using UnityEngine;

namespace _Logic.Gameplay.Items
{
    [CreateAssetMenu(menuName = nameof(SlotsPreset), fileName = nameof(SlotsPreset), order = 0)]
    public class SlotsPreset : ScriptableObject
    {
        [field: SerializeField] public List<SlotType> PossibleSlots { get; private set; }
        [field: SerializeField] public List<SlotType> BlockingSlots { get; private set; }
    }
}