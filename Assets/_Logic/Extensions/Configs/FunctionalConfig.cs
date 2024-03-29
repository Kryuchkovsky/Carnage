using System;
using System.Collections.Generic;
using System.Linq;
using _Logic.Extensions.CodeGenerator;
using UnityEditor;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public abstract class FunctionalConfig<TEnumType, TData> : Config, IExpandedConfig 
        where TEnumType : struct, Enum
        where TData : Data<TEnumType> 
    {
        [SerializeField] protected List<TData> _dataList;

        private Dictionary<int, TData> _dataDictionary;
        
        public override void Initialize()
        {
            _dataDictionary = new Dictionary<int, TData>();

            for (int i = 0; i < _dataList.Count; i++)
            {
                _dataDictionary.TryAdd(Convert.ToInt32(_dataList[i].Type), _dataList[i]);
                _dataList[i].Initialize(i);
            }
        }

        public bool HasData(int type) => _dataDictionary.ContainsKey(type);

        public TData GetData(int type) => _dataDictionary[type];

        [ContextMenu("FindAllDataObjects")]
        public void FindAllDataObjects()
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(TData).Name);
            _dataList.Clear();

            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<TData>(path);
                _dataList.Add(asset);
            }

            EditorUtility.SetDirty(this);
        }

        [ContextMenu("GenerateDataEnumTypes")]
        public void GenerateDataEnumTypes(bool useOldValues = true)
        {
            EnumGenerator.GenerateEnumValues<TData, TEnumType>(useOldValues);
        }

        public void UpdateDataTypes()
        {
            foreach (var data in _dataList)
            {
                var stringValue = Enum.GetNames(typeof(TEnumType)).FirstOrDefault(e => e == data.name);
                var enumValue = Enum.Parse<TEnumType>(stringValue);
                var property = data.GetType().BaseType.GetProperty(nameof(data.Type));
                property.SetValue(data, enumValue);
                EditorUtility.SetDirty(data);
                AssetDatabase.SaveAssetIfDirty(data);
            }
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }
}