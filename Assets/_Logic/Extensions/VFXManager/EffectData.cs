using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    [CreateAssetMenu(menuName = "Effects/Create EffectData", fileName = "EffectData", order = 0)]
    public class EffectData : Data<VFXType>
    {
        [field: SerializeField] public Effect Effect { get; private set; }
    }
}