using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    [CreateAssetMenu(menuName = "VFX/Create VFXCatalog", fileName = "VFXCatalog", order = 0)]
    public class VFXCatalog : FunctionalConfig<VFXType, VFXData>
    {
    }
}