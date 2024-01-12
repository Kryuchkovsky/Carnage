using System.Collections.Generic;
using System.Linq;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public class UnitCatalog : InitializableConfig
    {
        [SerializeField] private List<UnitProvider> _units;

        private Dictionary<string, UnitProvider> _unitsDictionary;
        
        public override void Initialize()
        {
            _unitsDictionary = _units.ToDictionary(p => p.Id, p => p);
        }

        public UnitProvider GetUnit(string id) => _unitsDictionary[id];
    }
}