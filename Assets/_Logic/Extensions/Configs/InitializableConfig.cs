using UnityEngine;

namespace _Logic.Extensions.Configs
{
    public abstract class InitializableConfig : ScriptableObject
    {
        public virtual void Initialize()
        {
        }
    }
}