using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Gameplay.SelectionPanel
{
    public class SelectionView : MonoBehaviour
    {
        public event Action<SelectionData> Selected;
        private SelectionData _selectionData;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public void Initialize(SelectionData selectionData)
        {
            _selectionData = selectionData;
            _descriptionText.SetText(_selectionData.Description);
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
            if (_selectionData == null) return;
            
            Selected?.Invoke(_selectionData);
        }
    }
}