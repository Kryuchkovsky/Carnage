using UnityEngine;

namespace _Logic.Gameplay.Units.Creatures
{
    [CreateAssetMenu(menuName = "Create CreaturesCatalog", fileName = "CreaturesCatalog", order = 0)]
    public class CreatureCatalog : UnitsCatalog<CreatureModel>
    {
        [field: SerializeField] public CreatureProvider CreatureProvider { get; private set; }
    }
}