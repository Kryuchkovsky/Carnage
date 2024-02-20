using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units.AI
{
    [CreateAssetMenu(menuName = "Create AISettings", fileName = "AISettings", order = 0)]
    public class AISettings : InitializableConfig
    {
        [field: SerializeField, Min(1)] public float TargetSearchRangeToAttackRangeRatio { get; private set; } = 3;
        [field: SerializeField, Min(1)] public float TargetFollowingRangeToAttackRangeRatio { get; private set; } = 1.5f;
    }
}