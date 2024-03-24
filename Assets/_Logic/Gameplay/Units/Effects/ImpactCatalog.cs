using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects
{
    [CreateAssetMenu(menuName = "Effects/Create ImpactCatalog", fileName = "ImpactCatalog", order = 0)]
    public class ImpactCatalog : FunctionalConfig<ImpactType, ImpactData>
    {
    }
}