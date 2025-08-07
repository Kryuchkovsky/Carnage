using _Logic.Gameplay.Units.Experience;
using _Logic.Gameplay.Units.Stats;
using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode
{
    public class GameplayUIContainer : MonoBehaviour
    {
        [field: SerializeField] public StatsPanel StatsPanel { get; private set; }
        [field: SerializeField] public PlayerExperienceBar PlayerExperienceBar { get; private set; }
        
        public void SetActivity(bool isActive) => gameObject.SetActive(isActive);
    }
}