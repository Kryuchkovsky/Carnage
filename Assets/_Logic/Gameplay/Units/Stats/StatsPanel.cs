using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Gameplay.Units.Stats
{
    public class StatsPanel : SingletonBehavior<StatsPanel>, IStatsPanel
    {
        [SerializeField] private StatView _statViewPrefab;
        
        private Dictionary<StatType, StatView> _views = new();
        private Dictionary<StatType, Stat> _stats = new();

        public void Initiate(Dictionary<StatType, Stat> stats)
        {
            _stats = stats;
            Update();
        }

        public void Update()
        {
            if (_stats != null)
            {
                foreach (var stat in _stats)
                {
                    SetStat(stat.Key, stat.Value.CurrentValue);
                }
            
                foreach (var view in _views)
                {
                    if (_stats.ContainsKey(view.Key)) continue;
                
                    RemoveStat(view.Key);
                }
            }
        }

        public void SetStat(StatType type, float value)
        {
            if (_views.ContainsKey(type))
            {
                _views[type].SetValue(value);
            }
            else
            {
                var view = Instantiate(_statViewPrefab, transform);
                view.Initiate(type.ToString(), value);
                _views.Add(type, view);
            }
        }
        
        public void RemoveStat(StatType type)
        {
            if (_views.ContainsKey(type))
            {
                Destroy(_views[type].gameObject);
                _views.Remove(type);
            }
        }
    }
}