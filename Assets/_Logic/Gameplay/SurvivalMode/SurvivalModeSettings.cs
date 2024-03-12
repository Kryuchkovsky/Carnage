using System.Collections.Generic;
using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode
{
    [CreateAssetMenu(menuName = "Create SurvivalModeSettings", fileName = "SurvivalModeSettings", order = 0)]
    public class SurvivalModeSettings : Config
    {
        [field: SerializeField] public List<UnitType> Units { get; private set; }
        [field: SerializeField] public float EnemySpawnInterval { get; private set; } = 5;
    }
}