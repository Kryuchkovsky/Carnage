using System;
using System.Collections.Generic;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Effects
{
    [CreateAssetMenu(menuName = "Effects/Create Impact", fileName = "Impact", order = 0)]
    public class ImpactData : Data<ImpactType>
    {
        [field: SerializeField] public List<EffectType> EffectForAll { get; private set; }
        [field: SerializeField] public List<EffectType> EffectForAllies { get; private set; }
        [field: SerializeField] public List<EffectType> EffectsForEnemies { get; private set; }
        [field: SerializeField] public ProjectileType ProjectileType { get; private set; } = ProjectileType.None;
        [field: SerializeField, Min(-1)] public int MaxTargets { get; private set; } = -1;
        
        [field: SerializeField] public AnimationCurve RadiusCurve { get; private set; } = AnimationCurve.Constant(0, 1, 1);
        
        [field: SerializeField, Min(-1)] public float BaseRadius { get; private set; } = -1;
        
        [field: SerializeField, Min(-1)] public float Duration { get; private set; }
        [field: SerializeField] public VFXType VFXType { get; private set; }
        
        [field: SerializeField] public string Description { get; private set; } = String.Empty;
        
        public float CalculateRadius(float time) => BaseRadius * RadiusCurve.Evaluate(time);
    }
}