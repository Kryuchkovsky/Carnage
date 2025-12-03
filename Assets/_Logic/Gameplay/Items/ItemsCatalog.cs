using _Logic.Extensions.Configs;
using _Logic.Gameplay.Projectiles.Components;
using UnityEngine;

namespace _Logic.Gameplay.Items
{
    [CreateAssetMenu(menuName = nameof(ItemsCatalog), fileName = nameof(ItemsCatalog), order = 0)]
    public class ItemsCatalog : FunctionalConfig<ItemCategory, ItemData>
    {

    }
}