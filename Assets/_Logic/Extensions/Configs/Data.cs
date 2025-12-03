using System;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    [Serializable]
    public abstract class Data<TEnumKey> : ScriptableObject where TEnumKey : Enum
    {
        [field: SerializeField] public virtual TEnumKey Type { get; private set; }
        
        public int Id => Convert.ToInt32(Type);

        public virtual void Initialize()
        {
        }
    }
}