using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Gameplay.Units.Stats
{
    public class StatView : MonoBehaviour
    {
        [field: SerializeField] private Image _icon;
        [field: SerializeField] private TextMeshProUGUI _nameText;
        [field: SerializeField] private TextMeshProUGUI _valueText;

        public void Initiate(string statName, float value)
        {
            _nameText.SetText(statName);
            SetValue(value);
        }

        public void SetValue(float value)
        {
            _valueText.SetText("{0:0}", value);
        }
    }
}