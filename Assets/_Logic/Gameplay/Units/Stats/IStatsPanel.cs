namespace _Logic.Gameplay.Units.Stats
{
    public interface IStatsPanel
    {
        public void SetStat(StatType type, float value);
        public void RemoveStat(StatType type);
    }
}