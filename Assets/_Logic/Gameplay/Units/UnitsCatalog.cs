using System.Collections.Generic;
using System.Linq;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitsCatalog<T> : InitializableConfig where T : UnitModel
    {
        [SerializeField] private List<BasicUnitData<T>> _unitData;

        private Dictionary<string, BasicUnitData<T>> _unitDataDictionary;
        
        public override void Initialize()
        {
            _unitDataDictionary = _unitData.ToDictionary(p => p.Id, p => p);
        }

        public BasicUnitData<T> GetUnitData(string id) => _unitDataDictionary[id];
    }
}