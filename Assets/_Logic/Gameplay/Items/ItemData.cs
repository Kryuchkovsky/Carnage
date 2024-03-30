using System;
using UnityEngine;

namespace _Logic.Gameplay.Items
{
    [Serializable]
    public class ItemData
    {
        [field: SerializeField] public ItemProvider Prefab { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
    }
}