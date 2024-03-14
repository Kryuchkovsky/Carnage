using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    [CreateAssetMenu(menuName = "VFX/Create VFXData", fileName = "VFXData", order = 0)]
    public class VFXData : Data<VFXType>
    {
        [field: SerializeField] public VFX VFX { get; private set; }
    }
}