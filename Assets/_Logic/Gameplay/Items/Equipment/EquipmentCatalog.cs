using System.Collections.Generic;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Items.Equipment
{
    [CreateAssetMenu(menuName = nameof(EquipmentCatalog), fileName = nameof(EquipmentCatalog), order = 0)]
    public class EquipmentCatalog : Config
    {
        [field: SerializeField] public List<EquipmentData> DataList { get; private set; } = new();

        private Dictionary<string, EquipmentData> _idDataDictionary = new();
        private Dictionary<EquipmentType, HashSet<EquipmentData>> _sortedByTypesDictionary = new();

        public override void Initialize()
        {
            base.Initialize();

            foreach (var data in DataList)
            {
                _idDataDictionary.Add(data.Id, data);

                if (_sortedByTypesDictionary.ContainsKey(data.EquipmentType))
                    _sortedByTypesDictionary[data.EquipmentType].Add(data);
                else _sortedByTypesDictionary.Add(data.EquipmentType, new HashSet<EquipmentData>{ data });
            }
        }

        public EquipmentData GetById(string id) => _idDataDictionary[id];
        public HashSet<EquipmentData> GetAllOfType(EquipmentType type) => _sortedByTypesDictionary[type];
    }
}