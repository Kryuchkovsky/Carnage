using System;
using System.Collections.Generic;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Items;
using _Logic.Gameplay.Units.Stats;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace _Logic.Gameplay.Equipment
{
    public abstract class EquipmentData : Data<EquipmentType>, IEquipmentStatBuff
    {
        [field: SerializeField, ReadOnly] public string Id { get; private set; }
        
        [field: SerializeField] public EquipmentProvider Prefab { get; private set; }
        [field: SerializeField] public List<StatModifier> StatModifiers { get; private set; }
        
        [field: SerializeField] public bool SetSlotsPreset { get; private set; }
        [field: SerializeField, HideIf(nameof(SetSlotsPreset))] public SlotType SlotType { get; private set; }
        [field: SerializeField, ShowIf(nameof(SetSlotsPreset))] public SlotsPreset SlotsPreset { get; private set; }

        public StatModificationType StatModificationType => StatModificationType.Replacement;
        public abstract EquipmentCategory Category { get; }

        private void OnValidate()
        {
            GenerateIdIfEmpty();
        }

        private void GenerateIdIfEmpty()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
                AssetDatabase.Refresh();
            }
        }
    }
}