using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    [CreateAssetMenu(menuName = "Create EffectsCatalog", fileName = "EffectsCatalog", order = 0)]
    public class EffectsCatalog : ScriptableObject
    {
        [field: SerializeField] public List<EffectData> Effects { get; private set; }
            
        [Serializable]
        public struct EffectData
        {
            public string Id;
            public Effect _effect;
        }
    }
}