using System;
using System.Collections.Generic;
using System.Linq;

namespace _Logic.Gameplay.Units.Stats
{
    [Serializable]
    public class StatStorage
    {
        private Dictionary<IStatBuff, StatModificationSource> _modifications = new();

        public Dictionary<StatType, Stat> Stats { get; private set; } = new();
        public bool IsModified { get; private set; }
        
        public void Add(StatType type, float value)
        {
            Stats.TryAdd(type, new Stat(type, value));
        }

        public void SetStat(Stat stat)
        {
            Stats[stat.StatType] = stat;
        }

        public bool HasStat(StatType statType) => Stats.ContainsKey(statType);
        
        public float GetCurrentValue(StatType statType) => Stats.TryGetValue(statType, out var stat) ? stat.CurrentValue : 0;

        public float GetBaseValue(StatType statType) => Stats.TryGetValue(statType, out var stat) ? stat.BaseValue : 0;

        public void AddBuff(IStatBuff buff)
        {
            if (_modifications.TryGetValue(buff, out var existedModification))
                existedModification.AddBuff(buff);
            else _modifications.Add(buff, new StatModificationSource(buff));

            IsModified = true;
        }

        public void RemoveBuff(IStatBuff source)
        {
            if (_modifications.Remove(source))
                IsModified = true;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _modifications.Count;)
            {
                var kvp = _modifications.ElementAt(i);
                kvp.Value.Update(deltaTime);

                if (kvp.Value.IsExpired)
                {
                    _modifications.Remove(kvp.Key);
                    IsModified = true;
                }
                else
                {
                    i++;
                }
            }

            if (IsModified)
            {
                RecalculateStatValue();
                IsModified = false;
            }
        }
        
        public void RecalculateStatValue()
        {
            foreach (var stat in Stats.Values)
            {
                stat.Reset();
                
                foreach (var modification in _modifications.Values)
                    modification.Apply(stat);
            }
        }

        public void Reset()
        {
            foreach (var stat in Stats.Values)
            {
                stat.Reset();
            }
        }
    }
}