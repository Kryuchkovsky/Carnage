using System.Collections.Generic;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.SelectionPanel
{
    public class SelectionGroup
    {
        public readonly List<SelectionData> Selections;
        public readonly Entity Entity;

        public SelectionGroup(List<SelectionData> selections, Entity entity)
        {
            Selections = selections;
            Entity = entity;
        }
    }
}