using System.Collections.Generic;
using System.Linq;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    [CreateAssetMenu]
    public class UnitCatalog : InitializableConfig
    {
        [SerializeField] private List<BasicUnitData> _unitData;

        private Dictionary<string, BasicUnitData> _unitDataDictionary;
        
        public override void Initialize()
        {
            _unitDataDictionary = _unitData.ToDictionary(p => p.Id, p => p);
        }

        public BasicUnitData GetUnitData(string id) => _unitDataDictionary[id];
    }
}