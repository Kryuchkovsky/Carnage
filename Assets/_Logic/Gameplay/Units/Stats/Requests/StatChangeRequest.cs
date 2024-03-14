using _Logic.Gameplay.Units.Effects;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Stats.Requests
{
    public struct StatChangeRequest : IRequestData
    {
        public Entity Entity;
        public StatType Type;
        public StatModifier Modifier;
    }
}