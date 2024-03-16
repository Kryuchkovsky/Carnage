using System.Collections.Generic;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode
{
    [CreateAssetMenu(menuName = "Create SurvivalModeSettings", fileName = "SurvivalModeSettings", order = 0)]
    public class SurvivalModeSettings : Config
    {
        [field: SerializeField] public List<UnitType> Allies { get; private set; }
        [field: SerializeField] public List<UnitType> Enemies { get; private set; }
        [field: SerializeField] public EffectType PlayerEnhancmentEffectType { get; private set; }
        [field: SerializeField] public float EnemySpawnInterval { get; private set; } = 5;
    }
}