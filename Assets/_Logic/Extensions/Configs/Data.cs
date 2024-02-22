using System;
using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public abstract class Data<TEnumKey> : ScriptableObject where TEnumKey : Enum
    {
        [field: SerializeField] public TEnumKey Type { get; private set; }
    }
}