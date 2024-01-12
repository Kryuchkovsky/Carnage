using UnityEngine;

namespace _Logic.Extensions.Configs
{
    [CreateAssetMenu]
    public abstract class InitializableConfig : ScriptableObject
    {
        public virtual void Initialize()
        {
        }
    }
}