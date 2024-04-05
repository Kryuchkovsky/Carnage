using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units.Experience
{
    [CreateAssetMenu(menuName = "Create ExperienceSettings", fileName = "ExperienceSettings", order = 0)]
    public class ExperienceSettings : Config
    {
        [SerializeField, Range(1, 1000)] private float _baseExperienceReward = 100;
        [SerializeField, Range(0.01f, 1)] private float _experienceTransitionFactor = 0.13f;
        [SerializeField, Range(1, 1000)] private float _baseLevelCost = 300;
        [SerializeField, Range(1, 3)] private float _baseRaisedToPower = 1.5f;
        
        [field: SerializeField] public VFXType LevelUpVFX { get; private set; }

        public float CalculateExperienceRewardForMurder(float unitExperience, int receivers = 1) 
            => (_baseExperienceReward + unitExperience * _experienceTransitionFactor) / Mathf.Clamp(receivers, 1, float.MaxValue);
        
        public float CalculateLevelCost(int level) 
            => _baseLevelCost * (Mathf.Pow(_baseRaisedToPower, Mathf.Clamp(level - 1, 0, float.MaxValue)) - 1);
    }
}