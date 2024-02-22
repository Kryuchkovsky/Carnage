using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public class ConfigsManager : SingletonBehavior<ConfigsManager>
    {
        [SerializeField] private List<Config> _configs;

        private Dictionary<Type, Config> _configDictionary;

        protected override void Init()
        {
            base.Init();

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
                }
            }
        }
        
        public static T GetConfig<T>() where T : ScriptableObject => Instance._configDictionary[typeof(T)] as T;
    }
}