using System;

namespace _Logic.Gameplay.RewardSelector
{
    public class Selection
    {
        public readonly Action Callback;
        public readonly string Description;

        public Selection(string description, Action callback)
        {
            Description = description;
            Callback = callback;
        }
    }
}