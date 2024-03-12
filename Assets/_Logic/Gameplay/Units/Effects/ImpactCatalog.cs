using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Impacts;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects
{
    [CreateAssetMenu(menuName = "Impacts/Create ImpactCatalog", fileName = "ImpactCatalog", order = 0)]
    public class ImpactCatalog : FunctionalConfig<ImpactType, Impact>
    {
    }
}