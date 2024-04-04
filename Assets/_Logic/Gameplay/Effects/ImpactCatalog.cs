using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Effects
{
    [CreateAssetMenu(menuName = "Effects/Create ImpactCatalog", fileName = "ImpactCatalog", order = 0)]
    public class ImpactCatalog : FunctionalConfig<ImpactType, ImpactData>
    {
        [field: SerializeField] public ImpactProvider ImpactPrefab { get; private set; }
    }
}