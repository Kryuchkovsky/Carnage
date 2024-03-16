namespace _Logic.Gameplay.Units.Stats
{
    public interface IStatGroup
    {
        public StatGroupType Type { get; }
        public IStatGroup GetCopy();
    }
}