using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using _Logic.Extensions.Configs;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    public class EffectsService : SingletonBehavior<EffectsService>
    {
        [SerializeField, Range(0, 1024)] private int _initialPoolsCapacity;
        [SerializeField] private bool _autoFillingIsEnabled;
        
        private Dictionary<EffectType, ObjectPool<Effect>> _effectsPools;
        private EffectsCatalog _effectsCatalog;

        protected override void Init()
        {
            base.Init();
            
            _effectsPools = new Dictionary<EffectType, ObjectPool<Effect>>();
            _effectsCatalog = ConfigsManager.GetConfig<EffectsCatalog>();
            
            foreach (var effectType in (EffectType[])Enum.GetValues(typeof(EffectType)))
            {
                if (!_effectsCatalog.HasData(effectType)) continue;
                
                var effect = _effectsCatalog.GetData(effectType);
                
                _effectsPools.Add(
                    effectType, 
                    new ObjectPool<Effect>(effect.Effect, _initialPoolsCapacity, _autoFillingIsEnabled, transform,
                        e =>
                        {
                            e.Played += () => _effectsPools[effectType].Return(e);
                        },
                        e =>
                        {
                            e.transform.parent = transform;
                            e.ParticleSystem.Play();
                        },
                        e =>
                        {
                            e.ParticleSystem.Stop();
                        }));
            }
        }

        public void CreateEffect(EffectType type, Vector3 position, Quaternion rotation = new())
        {
            var effect = _effectsPools[type].Take();
            effect.transform.position = position;
            effect.transform.rotation = rotation;
            effect.ParticleSystem.Play();
        }
    }
}