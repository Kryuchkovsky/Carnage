using _Logic.Gameplay.Units.Creatures;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    [CreateAssetMenu]
    public class CreaturesCatalog : UnitsCatalog<CreatureModel>
    {
        [field: SerializeField] public CreatureProvider CreatureProvider { get; private set; }
    }
}