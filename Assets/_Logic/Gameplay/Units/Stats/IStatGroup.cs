namespace _Logic.Gameplay.Units.Stats
{
    public interface IStatGroup
    {
        public StatGroupType Type { get; }
        public void Update(float delta);
        public IStatGroup GetCopy();
    }
}