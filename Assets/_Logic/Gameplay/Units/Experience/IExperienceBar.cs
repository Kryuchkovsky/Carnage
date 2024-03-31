namespace _Logic.Gameplay.Units.Experience
{
    public interface IExperienceBar
    {
        public void SetFilling(float filling);
        public void SetExperienceAmount(float current, float maxValue);
        public void SetLevel(int level);
    }
}