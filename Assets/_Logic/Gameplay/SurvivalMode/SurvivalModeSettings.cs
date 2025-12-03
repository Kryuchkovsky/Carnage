using System.Collections.Generic;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Effects;
using _Logic.Gameplay.Items.Components;
using _Logic.Gameplay.Items.Equipment;
using _Logic.Gameplay.Units;
using _Logic.Gameplay.Units.Stats;
using Scellecs.Morpeh.Collections;
using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode
{
    [CreateAssetMenu(menuName = "Create SurvivalModeSettings", fileName = "SurvivalModeSettings", order = 0)]
    public class SurvivalModeSettings : Config
    {
        [field: SerializeField] public EquipmentData TestPlayerEquipmentData { get; private set; }
        [field: SerializeField] public List<UnitType> Allies { get; private set; }
        [field: SerializeField] public List<UnitType> Enemies { get; private set; }
        [field: SerializeField] public EffectType PlayerEnhancmentEffectType { get; private set; }
        [field: SerializeField, Range(0, 100)] public int MaxEnemiesNumber { get; private set; } = 30;
        [field: SerializeField, Range(0, 10)] public float EnemySpawnInterval { get; private set; } = 5;
        
        [field: SerializeField] public FastList<StatModifier> PossibleStatModifiersWhenLevelUp { get; private set; }
        [field: SerializeField] public List<ImpactType> PossibleImpactsWhenLevelUp { get; private set; }
        [field: SerializeField, Range(1, 6)] public int RewardsNumberWhenLevelUp { get; private set; } = 4;
        [field: SerializeField] public CollectorComponent CollectorComponent { get; private set; } = new()
        {
            Radius = 30,
            Speed = 10,
        };
    }
}