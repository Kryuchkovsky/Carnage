using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects.Actions
{
    [CreateAssetMenu(menuName = "Create " + nameof(StatChangeEffect), fileName = "StatChangeEffect", order = 0)]
    public class StatChangeEffect : Effect
    {
        [field: SerializeField] public StatType StatType { get; private set; }
    }
}