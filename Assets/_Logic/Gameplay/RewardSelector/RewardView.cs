using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Gameplay.RewardSelector
{
    public class RewardView : MonoBehaviour
    {
        public event Action<Selection> Selected;
        private Selection _selection;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public void Initialize(Selection selection)
        {
            _selection = selection;
            _descriptionText.SetText(_selection.Description);
        }

        private void Awake()
        {
            _button.onClick.AddListener(OnSelected);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnSelected);
        }

        private void OnSelected()
        {
            if (_selection == null) return;
            
            Selected?.Invoke(_selection);
        }
    }
}