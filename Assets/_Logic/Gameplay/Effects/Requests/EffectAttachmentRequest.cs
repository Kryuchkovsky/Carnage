using Scellecs.Morpeh;

namespace _Logic.Gameplay.Effects.Requests
{
    public struct EffectAttachmentRequest : IRequestData
    {
        public Entity TargetEntity;
        public EffectType EffectType;
    }
}