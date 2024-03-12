using _Logic.Gameplay.Units.Attack;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects.Actions
{
    [CreateAssetMenu(menuName = "Create " + nameof(PeriodicDamageEffect), fileName = "PeriodicDamageEffect", order = 0)]
    public class PeriodicDamageEffect : Effect
    {
        [field: SerializeField] public DamageType DamageType { get; private set; }
    }
}