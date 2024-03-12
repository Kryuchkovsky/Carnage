using _Logic.Gameplay.Units.Effects.Actions;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Effects.Requests
{
    public struct EffectAttachmentRequest : IRequestData
    {
        public Entity TargetEntity;
        public Effect Action;
    }
}