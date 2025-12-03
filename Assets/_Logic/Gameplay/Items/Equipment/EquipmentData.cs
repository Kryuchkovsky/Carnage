using System;
using _Logic.Gameplay.Units.Stats;
using Scellecs.Morpeh.Collections;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace _Logic.Gameplay.Items.Equipment
{
    [CreateAssetMenu(menuName = nameof(EquipmentData), fileName = nameof(EquipmentData), order = 0)]
    public class EquipmentData : ScriptableObject, IEquipmentStatBuff
    {
        [field: SerializeField, ReadOnly] public string Id { get; private set; }
        
        [field: SerializeField] public EquipmentProvider Prefab  { get; private set; }
        [field: SerializeField] public EquipmentType EquipmentType { get; private set; }
        [field: SerializeField] public FastList<StatModifier> StatModifiers { get; private set; }
        
        [field: SerializeField] public bool SetSlotsPreset { get; private set; }
        [field: SerializeField, HideIf(nameof(SetSlotsPreset))] public SlotType SlotType { get; private set; }
        [field: SerializeField, ShowIf(nameof(SetSlotsPreset))] public SlotsPreset SlotsPreset { get; private set; }

        public StatModificationType StatModificationType => StatModificationType.Replacement;

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