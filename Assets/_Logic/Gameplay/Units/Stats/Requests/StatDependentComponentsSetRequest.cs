using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Stats.Requests
{
    public struct StatDependentComponentsSetRequest : IRequestData
    {
        public Entity Entity;
        public bool HasReset;
    }
}