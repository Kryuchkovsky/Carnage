using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Units.Experience
{
    public class ExperienceSettings : Config
    {
        [SerializeField, Range(1, 1000)] private float _baseExperienceReward = 100;
        [SerializeField, Range(0.01f, 1)] private float _experienceMultiplier = 0.13f;
        [SerializeField, Range(1, 1000)] private float _baseLevelCost = 300;
        [SerializeField, Range(1, 3)] private float _baseRaisedToPower = 1.5f;

        public float CalculateExperienceAmountForKillingUnit(int unitExperience, int receivers = 1) 
            => (_baseExperienceReward + unitExperience * _experienceMultiplier) / Mathf.Clamp(receivers, 1, int.MaxValue);
        
        public float CalculateRequiredExperienceAmountForLevel(int level) 
            => _baseLevelCost * Mathf.Pow(_baseRaisedToPower, Mathf.Clamp(level - 1, 0, int.MaxValue));

        public int GetLevelByExperienceAmount(float experience)
        {
            for (int i = 1; i < 10000; i++)
            {
                var requiredExperience = CalculateRequiredExperienceAmountForLevel(i);

                if (experience <= requiredExperience)
                {
                    return i;
                }
            }

            return 1;
        }
    }
}