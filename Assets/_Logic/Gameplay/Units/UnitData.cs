using System.Collections.Generic;
using _Logic.Extensions.Attributes;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    [CreateAssetMenu(menuName = "Create UnitData", fileName = "UnitData")]
    public class UnitData : Data<UnitType>
    {
        #region Attack

        [SerializeField] private bool _hasAttack = true;
        
        [SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        private float _baseAttackDamageFactor = 10;

        [SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        private float _baseAttackSpeed = 100;
        
        [SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        private float _baseAttackRange = 1;
        
        [SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        private float _baseAttackTime = 2;

        #endregion

        #region Health

        [SerializeField] private bool _hasHealth = true;
        
        [SerializeField, ConditionalField(nameof(_hasHealth), true)] 
        private float _healthRegenerationRate;
        
        [SerializeField, ConditionalField(nameof(_hasHealth), true)] 
        private float _maxHeath = 50;
        
        [field: SerializeField, ConditionalField(nameof(_hasHealth), true)] 
        public VFXType DamageVFXType { get; private set; }
        
        [field: SerializeField, ConditionalField(nameof(_hasHealth), true)] 
        public VFXType DeathVFXType { get; private set; }

        #endregion

        #region Movement
        
        [SerializeField] private bool _hasMovement = true;
        
        [SerializeField, ConditionalField(nameof(_hasMovement), true)] 
        private float _movementSpeed = 10;
        
        [SerializeField, ConditionalField(nameof(_hasMovement), true)] 
        private float _rotationSpeed;

        #endregion

        #region Other

        [field: SerializeField, Min(1)] 
        public float VisionRange { get; private set; } = 25;

        #endregion
        
        [field: SerializeField] public UnitModel Model { get; private set; }
        
        [field: SerializeField, Min(0)] public float SpawnTime { get; private set; } = 3f;
        
        public Dictionary<StatType, float> Stats { get; private set; } = new ();

        public override void Initialize()
        {
            Model.Initialize(Id);
            
            if (_hasAttack)
            {
                Stats.TryAdd(StatType.AttackDamage, _baseAttackDamageFactor);
                Stats.TryAdd(StatType.AttackSpeed, _baseAttackSpeed);
                Stats.TryAdd(StatType.AttackRange, _baseAttackRange);
                Stats.TryAdd(StatType.AttackTime, _baseAttackTime);
            }
            
            if (_hasHealth)
            {
                Stats.TryAdd(StatType.HealthRegenerationRate, _healthRegenerationRate);
                Stats.TryAdd(StatType.MaxHeath, _maxHeath);
            }
            
            if (_hasMovement)
            {
                Stats.TryAdd(StatType.MovementSpeed, _movementSpeed);
                Stats.TryAdd(StatType.RotationSpeed, _rotationSpeed);
            }

            Stats.TryAdd(StatType.VisionRange, VisionRange);
        }
    }
}