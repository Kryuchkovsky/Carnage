using System.Collections.Generic;

namespace _Logic.Gameplay.Rewards
{
    public class SelectionGroup
    {
        public readonly List<Selection> Selections;

        public SelectionGroup(List<Selection> selections)
        {
            Selections = selections;
        }
    }
}