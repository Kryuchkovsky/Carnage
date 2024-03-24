using System.Collections.Generic;
using _Logic.Extensions.Attributes;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects
{
    [CreateAssetMenu(menuName = "Effects/Create EffectData", fileName = "EffectData", order = 0)]
    public class EffectData : Data<EffectType>
    {
        [field: SerializeField, Header("Affections")] 
        public List<EffectAffection> Changes { get; private set; }

        [field: SerializeField, Header("Health")] 
        public bool IsChangingHealth { get; private set; }
        
        [field: SerializeField, ConditionalField(nameof(IsChangingHealth), true)] 
        public HealthChangeData HealthChangeData { get; private set; }
        
        [field: SerializeField, ConditionalField(nameof(IsChangingHealth), true)] 
        public StatModifierOperationType HealthChangeOperationType { get; private set; }
        
        [field: SerializeField, ConditionalField(nameof(IsChangingHealth), true)] 
        public float HealthChangeInterval { get; private set; }
        
        [field: SerializeField, Header("Duration")] public bool IsPersist { get; private set; }
        
        [field: SerializeField, ConditionalField(nameof(IsPersist), false)] 
        public float Duration { get; private set; }

        [field: SerializeField, Header("Visualization")] 
        public VFXType VFXType { get; private set; }
    }
}