using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEditor;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public class ConfigManager : SingletonBehavior<ConfigManager>
    {
        [SerializeField] private List<Config> _configs;

        private Dictionary<Type, Config> _configDictionary;

        protected override void Initialize()
        {
            base.Initialize();

            _configDictionary = new Dictionary<Type, Config>();
            
            foreach (var config in _configs)
            {
                config.Initialize();
                _configDictionary.Add(config.GetType(),config);
            }
        }

        public static void FillConfigs()
        {
            foreach (var config in Instance._configs)
            {
                if (config is IExpandedConfig)
                {
                    var expandedConfig = (IExpandedConfig)config;
                    expandedConfig.FindAllDataObjects();
                    expandedConfig.UpdateDataTypes();
                    EditorUtility.SetDirty(config);
                }
            }
            
            AssetDatabase.Refresh();
        }
        
        public static void GenerateDataEnumTypes()
        {
            foreach (var config in Instance._configs)
            {
                if (config is IExpandedConfig)
                {
                    var expandedConfig = (IExpandedConfig)config;
                    expandedConfig.GenerateDataEnumTypes();
                    expandedConfig.UpdateDataTypes();
                    EditorUtility.SetDirty(config);
                }
            }
            
            AssetDatabase.Refresh();
        }
        
        public T GetConfig<T>() where T : ScriptableObject => Instance._configDictionary[typeof(T)] as T;
    }
}