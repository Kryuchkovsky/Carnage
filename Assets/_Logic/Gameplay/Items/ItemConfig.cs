using System.Collections.Generic;
using System.Linq;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Projectiles.Components;
using UnityEngine;

namespace _Logic.Gameplay.Items
{
    [CreateAssetMenu(menuName = "Create ItemConfig", fileName = "ItemConfig", order = 0)]
    public class ItemConfig : Config
    {
        [SerializeField] private List<ItemData> _items;
        
        private Dictionary<ItemType, ItemData> _itemDictionary;

        [field: SerializeField]
        public FlightParametersComponent FlightParameters { get; private set; } = new()
        {
            Speed = 30,
            FlightRangeToHeightRatio = 0.3f
        };

        [field: SerializeField, Range(1, 30)] 
        public float CollectionRange { get; private set; } = 10;

        public override void Initialize()
        {
            base.Initialize();
            _itemDictionary = _items.ToDictionary(i => i.ItemType, i => i);
        }

        public ItemData GetItemData(ItemType type) => _itemDictionary[type];
    }
}