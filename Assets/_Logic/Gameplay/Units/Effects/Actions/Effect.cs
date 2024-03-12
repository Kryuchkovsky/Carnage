using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Impacts;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects.Actions
{
    public abstract class Effect : Data<EffectType>
    {
        [field: SerializeField] public VFXType VFXType { get; private set; }
        [field: SerializeField] public float Strength { get; private set; }
        [field: SerializeField, Min(-1)] public float Duration { get; private set; }
    }
}