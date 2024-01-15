using _Logic.Core;
using _Logic.Gameplay.Units.Health;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    public sealed class DamageRequestsProcessingSystem : AbstractSystem
    {
        public override void OnAwake()
        {
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in World.GetRequest<DamageRequest>().Consume())   
            {
                World.GetRequest<HealthChangeRequest>().Publish(new HealthChangeRequest
                {
                    Entity = request.ReceiverEntity,
                    Change = -request.Damage
                });
            }
        }
    }
}