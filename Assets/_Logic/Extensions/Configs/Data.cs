using System;
using TriInspector;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public abstract class Data<TEnumKey> : ScriptableObject where TEnumKey : Enum
    {
        [field: SerializeField, ReadOnly] public TEnumKey Type { get; private set; }
        [field: SerializeField, ReadOnly] public int Id { get; private set; }

        public virtual void Initialize(int id)
        {
            Id = id;
        }
    }
}