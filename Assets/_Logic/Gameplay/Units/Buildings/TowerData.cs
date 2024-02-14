using _Logic.Gameplay.Units.Attack;
using _Logic.Gameplay.Units.Health;
using UnityEngine;

namespace _Logic.Gameplay.Units.Buildings
{
    [CreateAssetMenu(menuName = "Buildings/Tower Data", fileName = "TowerData")]
    public class TowerData : ScriptableObject
    {
        [field: SerializeField] public string ProjectileId { get; private set; }
        [field: SerializeField] public AttackData AttackData { get; private set; }
        [field: SerializeField] public HealthData HealthData { get; private set; }
    }
}