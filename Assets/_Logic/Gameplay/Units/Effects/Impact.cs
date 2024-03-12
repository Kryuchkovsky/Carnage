using System.Collections.Generic;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Effects.Actions;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects
{
    [CreateAssetMenu(menuName = "Impacts/Create Impact", fileName = "Impact", order = 0)]
    public class Impact : Data<ImpactType>
    {
        [field: SerializeField] public List<Effect> EffectForAll { get; private set; }
        [field: SerializeField] public List<Effect> EffectForAllies { get; private set; }
        [field: SerializeField] public List<Effect> EffectsForEnemies { get; private set; }
        [field: SerializeField, Min(-1)] public int MaxTargets { get; private set; } = -1;
        [field: SerializeField, Min(0)] public float Radius { get; private set; }
    }
}