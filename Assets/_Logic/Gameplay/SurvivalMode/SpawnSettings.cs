using System.Collections.Generic;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode
{
    [CreateAssetMenu(menuName = nameof(SpawnSettings), fileName = nameof(SpawnSettings), order = 0)]
    public class SpawnSettings : Config
    {
        [field: SerializeField] public Difficulty Difficulty { get; private set; }

        [field: SerializeField]
        public List<StatType> EnchancedEnemiesStats { get; private set; } = new()
        {
            StatType.AttackDamage,
            StatType.MaxHeath
        };

        [field: SerializeField] public float BaseSpawnRate { get; private set; } = 1;
        [field: SerializeField] public float StatMultiplierForSecond { get; private set; } = 0.001f;
        [field: SerializeField] public float StatMultiplierForWave { get; private set; } = 0.05f;
        [field: SerializeField] public float WaveDuration { get; private set; } = 30;
        [field: SerializeField] public float IntervalBetweenWaves { get; private set; } = 5;
        [field: SerializeField, Range(0, 100)] public int MaxEnemiesNumber { get; private set; } = 30;
    }
}