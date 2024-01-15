using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI
{
    [CreateAssetMenu]
    public class AISettings : InitializableConfig
    {
        [field: SerializeField, Min(1)] public float TargetSearchingRangeToAttackRangeRatio { get; private set; } = 3;
    }
}