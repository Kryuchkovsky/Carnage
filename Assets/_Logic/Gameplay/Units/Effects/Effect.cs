using System.Collections.Generic;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Attack;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects
{
    [CreateAssetMenu(menuName = "Create Effect", fileName = "Effect", order = 0)]
    public class Effect : Data<EffectType>
    {
        [field: SerializeField] public List<StatChange> Changes { get; private set; }

        [field: SerializeField] public DamageType DamageType { get; private set; }

        [field: SerializeField] public VFXType VFXType { get; private set; }
        [field: SerializeField, Min(-1)] public float Duration { get; private set; }
    }
}