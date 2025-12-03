using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Items
{
    public abstract class ItemData : Data<ItemCategory>
    {
        [field: SerializeField] public ItemProvider Prefab { get; private set; }
    }
}