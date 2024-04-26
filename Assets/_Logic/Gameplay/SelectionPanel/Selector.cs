using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Gameplay.SelectionPanel
{
    public class Selector : SingletonBehavior<Selector>
    {
        public event Action<SelectionData> Selected;
        
        [SerializeField] private SelectionView _selectionViewPrefab;

        private readonly Queue<SelectionGroup> _selectionGroups = new();
        private readonly List<SelectionView> _selectionViews = new();

        public void AddToQueue(SelectionGroup group)
        {
            _selectionGroups.Enqueue(group);
            Show();
        }

        public void Clear()
        {
            _selectionGroups.Clear();
            Hide();
        }

        private void Show()
        {
            var group = _selectionGroups.Peek();

            if (group.Selections.Count > _selectionViews.Count)
            {
                while (group.Selections.Count != _selectionViews.Count)
                {
                    var view = Instantiate(_selectionViewPrefab, transform);
                    view.Selected += HandleSelection;
                    _selectionViews.Add(view);
                }
            }
            
            for (int i = 0; i < _selectionViews.Count; i++)
            {
                _selectionViews[i].gameObject.SetActive(i < group.Selections.Count);

                if (i < group.Selections.Count)
                {
                    _selectionViews[i].Initialize(group.Selections[i]);
                }
            }
        }

        private void Hide()
        {
            foreach (var view in _selectionViews)
            {
                view.gameObject.SetActive(false);
            }
        }

        private void HandleSelection(SelectionData selectionData)
        {
            var group = _selectionGroups.Dequeue();
            selectionData.Action?.Invoke(group.Entity);
            Selected?.Invoke(selectionData);

            if (_selectionGroups.Count > 0)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}