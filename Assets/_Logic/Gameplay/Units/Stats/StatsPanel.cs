using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Gameplay.Units.Stats
{
    public class StatsPanel : SingletonBehavior<StatsPanel>, IStatsPanel
    {
        [SerializeField] private StatView _statViewPrefab;
        
        private Dictionary<StatType, StatView> _views = new();

        public void Initiate(Dictionary<StatType, Stat> stats)
        {
            foreach (var stat in stats)
            {
                SetStat(stat.Key, stat.Value.CurrentValue);
            }
            
            foreach (var view in _views)
            {
                if (stats.ContainsKey(view.Key)) continue;
                
                RemoveStat(view.Key);
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