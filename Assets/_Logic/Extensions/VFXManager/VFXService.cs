using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using _Logic.Extensions.Configs;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    public class VFXService : SingletonBehavior<VFXService>
    {
        [SerializeField, Range(0, 1024)] private int _initialPoolsCapacity;
        [SerializeField] private bool _autoFillingIsEnabled;
        
        private Dictionary<VFXType, ObjectPool<VFX>> _vfxPools;
        private VFXCatalog _vfxCatalog;

        protected override void Initialize()
        {
            base.Initialize();
            
            _vfxPools = new Dictionary<VFXType, ObjectPool<VFX>>();
            _vfxCatalog = ConfigManager.Instance.GetConfig<VFXCatalog>();
            
            foreach (var effectType in (VFXType[])Enum.GetValues(typeof(VFXType)))
            {
                if (!_vfxCatalog.HasData((int)effectType)) continue;
                
                var effect = _vfxCatalog.GetData((int)effectType);
                
                _vfxPools.Add(
                    effectType, 
                    new ObjectPool<VFX>(effect.VFX, _initialPoolsCapacity, _autoFillingIsEnabled, transform,
                        e =>
                        {
                            e.Played += () => _vfxPools[effectType].Return(e);
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

        public void CreateEffect(VFXType type, Vector3 position, Quaternion rotation = new())
        {
            var effect = _vfxPools[type].Take();
            effect.transform.position = position;
            effect.transform.rotation = rotation;
            effect.ParticleSystem.Play();
        }
    }
}