using _Logic.Core;
using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Effects.Actions;
using _Logic.Gameplay.Units.Effects.Requests;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Effects.Systems
{
    public class EffectAttachmentRequestsProcessingSystem : AbstractUpdateSystem
    {
        private Request<EffectAttachmentRequest> _request;
        private GameEffectCatalog _gameEffectCatalog;

        public override void OnAwake()
        {
            _request = World.GetRequest<EffectAttachmentRequest>();
            _gameEffectCatalog = ConfigManager.GetConfig<GameEffectCatalog>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                switch (request.Action)
                {
                    
                }
            }
        }
    }
}