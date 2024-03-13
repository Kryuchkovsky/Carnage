using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    [CreateAssetMenu(menuName = "Effects/Create EffectsCatalog", fileName = "EffectsCatalog", order = 0)]
    public class EffectsCatalog : FunctionalConfig<VFXType, EffectData>
    {
    }
}