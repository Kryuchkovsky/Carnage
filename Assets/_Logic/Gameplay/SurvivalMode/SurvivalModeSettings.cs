using System.Collections.Generic;
using System.Linq;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Effects;
using _Logic.Gameplay.Equipment;
using _Logic.Gameplay.Items.Components;
using _Logic.Gameplay.Units;
using _Logic.Gameplay.Units.Stats;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode
{
    [CreateAssetMenu(menuName = nameof(SurvivalModeSettings), fileName = nameof(SurvivalModeSettings), order = 0)]
    public class SurvivalModeSettings : Config
    {
        [field: SerializeField] public EquipmentData TestPlayerEquipmentData { get; private set; }
        [field: SerializeField] public List<UnitType> Allies { get; private set; }
        [field: SerializeField] public List<UnitType> Enemies { get; private set; }
        [field: SerializeField] public List<SpawnSettings> SpawnSettings { get; private set; }
        [field: SerializeField] public EffectType PlayerEnhancmentEffectType { get; private set; }

        [field: SerializeField] public FastList<StatModifier> PossibleStatModifiersWhenLevelUp { get; private set; }
        [field: SerializeField] public List<ImpactType> PossibleImpactsWhenLevelUp { get; private set; }
        [field: SerializeField, Range(1, 6)] public int RewardsNumberWhenLevelUp { get; private set; } = 4;
        [field: SerializeField] public CollectorComponent CollectorComponent { get; private set; } = new()
        {
            Radius = 30,
            Speed = 10,
        };

        public SpawnSettings GetSpawnSettings(Difficulty difficulty)
        {
            return SpawnSettings.FirstOrDefault(s => s.Difficulty == difficulty);
        }
    }
}