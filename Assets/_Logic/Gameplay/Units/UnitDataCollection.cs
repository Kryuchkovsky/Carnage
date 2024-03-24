using System;
using System.Collections.Generic;
using _Logic.Extensions.Attributes;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Attack;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Movement;
using _Logic.Gameplay.Units.Spawn;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    [CreateAssetMenu(menuName = "Create UnitDataCollection", fileName = "UnitDataCollection")]
    public class UnitDataCollection : Data<UnitType>
    {
        private Dictionary<Type, IStatGroup> _dataCatalog;

        [SerializeField] private UnitData<AttackStats> _attackData;
        [SerializeField] private UnitData<HealthStats> _healthData;
        [SerializeField] private UnitData<MovementStats> _movementData;
        [SerializeField] private UnitData<SpawnAbilityData> _spawnAbilityData;
        
        [field: SerializeField] public UnitModel Model { get; private set; }
        [field: SerializeField, Min(0)] public float SpawnTime { get; private set; } = 3f;

        public override void Initialize()
        {
            base.Initialize();
            
            _dataCatalog = new Dictionary<Type, IStatGroup>();

            if (_attackData.Has)
            {
                _dataCatalog.Add(_attackData.Data.GetType(), _attackData.Data);
            }
            
            if (_healthData.Has)
            {
                _dataCatalog.Add(_healthData.Data.GetType(), _healthData.Data);
            }
            
            if (_movementData.Has)
            {
                _dataCatalog.Add(_movementData.Data.GetType(), _movementData.Data);
            }
            
            if (_spawnAbilityData.Has)
            {
                _dataCatalog.Add(_spawnAbilityData.Data.GetType(), _spawnAbilityData.Data);
            }
        }

        public bool TryGetData<T>(out T data) where T : IStatGroup
        {
            var hasData = _dataCatalog.TryGetValue(typeof(T), out var gottenData);
            data = hasData ? (T)gottenData.GetCopy() : default;
            return hasData;
        }
        
        [Serializable]
        private class UnitData<T> where T : IStatGroup
        {
            public bool Has;
        
            [ConditionalField(nameof(Has), true)]
            public T Data;
        }
    }
}