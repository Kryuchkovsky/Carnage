using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public class ConfigsManager : SingletonBehavior<ConfigsManager>
    {
        [SerializeField] private List<InitializableConfig> _configs;

        private Dictionary<Type, InitializableConfig> _configDictionary;

        protected override void Init()
        {
            base.Init();

            _configDictionary = new Dictionary<Type, InitializableConfig>();
            
            foreach (var config in _configs)
            {
                config.Initialize();
                _configDictionary.Add(config.GetType(),config);
            }
        }

        public static T GetConfig<T>() where T : ScriptableObject => Instance._configDictionary[typeof(T)] as T;
    }
}