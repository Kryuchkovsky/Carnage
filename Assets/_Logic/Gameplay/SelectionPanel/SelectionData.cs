using System;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.SelectionPanel
{
    public class SelectionData
    {
        public readonly Action<Entity> Action;
        public readonly string Description;

        public SelectionData(string description, Action<Entity> action)
        {
            Description = description;
            Action = action;
        }
    }
}