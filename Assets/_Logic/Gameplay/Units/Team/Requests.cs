using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Team
{
    public struct TeamDataSettingRequest : IRequestData
    {
        public Entity Entity;
        public int TeamId;
    }
}