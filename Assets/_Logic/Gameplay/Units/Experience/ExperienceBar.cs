using _GameLogic.Extensions.Patterns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Gameplay.Units.Experience
{
    public class ExperienceBar : SingletonBehavior<ExperienceBar>
    {
        [SerializeField] private Image _fillingImage;
        [SerializeField] private TextMeshProUGUI _levelText;

        public void SetExperienceBarFilling(float filling) => _fillingImage.fillAmount = filling;
        public void SetLevel(int level) => _levelText.SetText("{0:0}", level);
    }
}