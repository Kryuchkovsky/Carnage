using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEditor;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public class ConfigManager : SingletonBehavior<ConfigManager>
    {
        private Dictionary<Type, Config> _configDictionary;
        [SerializeField] private List<Config> _configs;

        protected override void Initialize()
        {
            if (IsInitiated)
                return;
            
            base.Initialize();

            _configDictionary = new Dictionary<Type, Config>();
            
            foreach (var config in _configs)
            {
                config.Initialize();
                _configDictionary.Add(config.GetType(),config);
            }
        }

        public IReadOnlyDictionary<Type, Config> GetConfigs()
        {
            if (!IsInitiated)
                Initialize();
                
            return _configDictionary;
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
                }
            }
            
            AssetDatabase.Refresh();
        }
        
        public static void GenerateDataEnumTypes(bool useOldValues = true)
        {
            foreach (var config in Instance._configs)
            {
                if (config is IExpandedConfig)
                {
                    var expandedConfig = (IExpandedConfig)config;
                    expandedConfig.GenerateDataEnumTypes(useOldValues);
                    expandedConfig.UpdateDataTypes();
                }
            }
            
            AssetDatabase.Refresh();
        }
        
        public T GetConfig<T>() where T : ScriptableObject => Instance._configDictionary[typeof(T)] as T;
    }
}