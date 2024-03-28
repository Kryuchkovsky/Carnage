using System;
using System.Collections.Generic;

namespace _Logic.Gameplay.Units.Stats
{
    [Serializable]
    public class StatStorage
    {
        public Dictionary<StatType, Stat> Stats { get; private set; } = new();
        public bool HasChangedStat { get; private set; }
        
        public void Register(StatType type, float value)
        {
            Stats.TryAdd(type, new Stat(value));
        }

        public bool HasStat(StatType statType) => Stats.ContainsKey(statType);
        
        public float GetCurrentValue(StatType statType) => Stats.TryGetValue(statType, out var stat) ? stat.CurrentValue : 0;

        public float GetBaseValue(StatType statType) => Stats.TryGetValue(statType, out var stat) ? stat.BaseValue : 0;

        public void AddModifier(StatType statType, StatModifier statModifier)
        {
            var result = Stats.TryGetValue(statType, out var stat);

            if (result)
            {
                stat.AddModifier(statModifier);
            }
        }

        public void RemoveModifier(StatType statType, int index)
        {
            var result = Stats.TryGetValue(statType, out var stat);

            if (result)
            {
                stat.RemoveModifier(index);
            }
        }

        public void Update(float deltaTime)
        {
            HasChangedStat = false;
            
            foreach (var stat in Stats.Values)
            {
                stat.Update(deltaTime);

                if (stat.IsChangedInLastUpdate)
                {
                    HasChangedStat = stat.IsChangedInLastUpdate;
                }
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