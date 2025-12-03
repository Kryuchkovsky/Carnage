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

        private Dictionary<TEnumType, TData> _dataDictionary;
        
        public override void Initialize()
        {
            _dataDictionary = new Dictionary<TEnumType, TData>();

            for (int i = 0; i < _dataList.Count; i++)
            {
                _dataDictionary.TryAdd(_dataList[i].Type, _dataList[i]);
                _dataList[i].Initialize();
            }
        }

        public bool HasData(TEnumType type) => _dataDictionary.ContainsKey(type);

        public TData GetData(TEnumType type) => _dataDictionary[type];

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
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }
}