using System;
using System.Collections.Generic;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    public class VFXService
    {
        private Dictionary<VFXType, ObjectPool<VFX>> _vfxPools = new();
        private VFXCatalog _vfxCatalog;
        
        
        public VFXService(VFXCatalog vfxCatalog)
        {
            _vfxCatalog = vfxCatalog;

            foreach (var effectType in (VFXType[])Enum.GetValues(typeof(VFXType)))
            {
                if (!_vfxCatalog.HasData((int)effectType)) continue;

                var effect = _vfxCatalog.GetData((int)effectType);
                var pool = new ObjectPool<VFX>(
                    prefab: effect.VFX,
                    capacity: 32,
                    takeAction: e =>
                    {
                        e.Played += OnPlayed;
                    },
                    returnAction: e =>
                    {
                        e.Played -= OnPlayed;
                    });
                
                _vfxPools.Add(effectType, pool);

                void OnPlayed(VFX vfx) => _vfxPools[effectType].Return(vfx);
            }
        }

        public void CreateEffect(VFXType type, Vector3 position, Quaternion rotation = new(), Transform parent = null)
        {
            if (type == VFXType.None) return;
            
            var effect = _vfxPools[type].Take();
            effect.Initialize(position, rotation);

            if (parent)
            {
                effect.transform.SetParent(parent);
            }
        }
    }
}