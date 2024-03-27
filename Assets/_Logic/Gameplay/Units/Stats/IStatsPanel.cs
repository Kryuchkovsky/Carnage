using System.Collections.Generic;

namespace _Logic.Gameplay.Units.Stats
{
    public interface IStatsPanel
    {
        public void Initiate(Dictionary<StatType, Stat> stats);
        public void Update();
        public void SetStat(StatType type, float value);
        public void RemoveStat(StatType type);
    }
}