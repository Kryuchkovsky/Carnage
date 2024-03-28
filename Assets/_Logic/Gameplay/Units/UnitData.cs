using System.Collections.Generic;
using _Logic.Extensions.Attributes;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Health;
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
        private float _attackDamage = 10;
        
        [SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        private float _attackRange = 1.5f;
        
        [SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        private float _attackSpeed = 100;
        
        [SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        private float _attackTime = 2;
        
        [field: SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        public ProjectileType ProjectileType { get; private set; }
        
        [field: SerializeField, ConditionalField(nameof(_hasAttack), true)] 
        public HealthChangeType AttackHealthChangeType { get; private set; }

        #endregion

        #region Health

        [SerializeField] private bool _hasHealth = true;
        
        [SerializeField, ConditionalField(nameof(_hasHealth), true)] 
        private float _healthRegenerationRate;
        
        [SerializeField, ConditionalField(nameof(_hasHealth), true)] 
        private float _maxHeath = 50;
        
        [field: SerializeField, ConditionalField(nameof(_hasHealth), true)] 
        public VFXType DamageVFXType { get; private set; }

        #endregion

        #region Movement
        
        [SerializeField] private bool _hasMovement = true;
        
        [SerializeField, ConditionalField(nameof(_hasMovement), true)] 
        private float _movementSpeed = 10;
        
        [SerializeField, ConditionalField(nameof(_hasMovement), true)] 
        private float _rotationSpeed;

        #endregion

        [field: SerializeField] public UnitModel Model { get; private set; }
        
        [field: SerializeField, Min(0)] public float SpawnTime { get; private set; } = 3f;
        
        public Dictionary<StatType, float> Stats { get; private set; } = new ();

        public override void Initialize(int id)
        {
            base.Initialize(id);
            Model.Initialize(id);
            
            if (_hasAttack)
            {
                Stats.Add(StatType.AttackDamage, _attackDamage);
                Stats.Add(StatType.AttackRange, _attackRange);
                Stats.Add(StatType.AttackSpeed, _attackSpeed);
                Stats.Add(StatType.AttackTime, _attackTime);
            }
            
            if (_hasHealth)
            {
                Stats.Add(StatType.HealthRegenerationRate, _healthRegenerationRate);
                Stats.Add(StatType.MaxHeath, _maxHeath);
            }
            
            if (_hasMovement)
            {
                Stats.Add(StatType.MovementSpeed, _movementSpeed);
                Stats.Add(StatType.RotationSpeed, _rotationSpeed);
            }

        }
    }
}