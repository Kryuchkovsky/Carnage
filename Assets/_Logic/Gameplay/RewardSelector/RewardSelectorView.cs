using System;
using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Gameplay.RewardSelector
{
    public class RewardSelectorView : SingletonBehavior<RewardSelectorView>
    {
        public event Action<Selection> RewardSelected;
        
        [SerializeField] private RewardView _rewardViewPrefab;

        private Queue<SelectionGroup> _selectionsGroups = new();
        private List<RewardView> _rewardViews = new();

        public void AddToQueue(SelectionGroup group)
        {
            _selectionsGroups.Enqueue(group);
            Show();
        }

        private void Show()
        {
            var group = _selectionsGroups.Peek();

            if (group.Selections.Count > _rewardViews.Count)
            {
                while (group.Selections.Count != _rewardViews.Count)
                {
                    var view = Instantiate(_rewardViewPrefab, transform);
                    view.Selected += HandleSelection;
                    _rewardViews.Add(view);
                }
            }
            
            for (int i = 0; i < _rewardViews.Count; i++)
            {
                _rewardViews[i].gameObject.SetActive(i < group.Selections.Count);

                if (i < group.Selections.Count)
                {
                    _rewardViews[i].Initialize(group.Selections[i]);
                }
            }
        }

        private void Hide()
        {
            foreach (var view in _rewardViews)
            {
                view.gameObject.SetActive(false);
            }
        }

        private void HandleSelection(Selection selection)
        {
            _selectionsGroups.Dequeue();

            if (_selectionsGroups.Count > 0)
            {
                Show();
            }
            else
            {
                Hide();
            }
            
            selection.Callback?.Invoke();
            RewardSelected?.Invoke(selection);
        }
    }
}