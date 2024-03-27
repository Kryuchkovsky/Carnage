using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    [CreateAssetMenu(menuName = "Create UnitsCatalog", fileName = "UnitsCatalog", order = 0)]
    public class UnitsCatalog : FunctionalConfig<UnitType, UnitData>
    {
        [field: SerializeField] public UnitProvider UnitProvider { get; private set; }
    }
}