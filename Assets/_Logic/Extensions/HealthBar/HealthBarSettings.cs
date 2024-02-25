using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Extensions.HealthBar
{
    [CreateAssetMenu(menuName = "Create HealthBarSettings", fileName = "HealthBarSettings", order = 0)]
    public class HealthBarSettings : Config
    {
        [field: SerializeField] public HealthBarView HealthBar { get; private set; }
        
        [field: SerializeField] public HealthBarColorData AlliedHealthBarColorData { get; private set; } = new(
            new Color32(148, 58, 59, 155),
            new Color32(33, 120, 15, 155),
            new Color32(92, 166, 77, 190)
        );

        [field: SerializeField] public HealthBarColorData EnemyTeamHealthBarColorData { get; private set; } = new(
            new Color32(230, 201, 29, 155),
            new Color32(87, 18, 19, 155),
            new Color32(148, 58, 59, 190)
        );

        [field: SerializeField, Range(0, 10)] public float HidingDelay { get; private set; } = 3;
    }
}