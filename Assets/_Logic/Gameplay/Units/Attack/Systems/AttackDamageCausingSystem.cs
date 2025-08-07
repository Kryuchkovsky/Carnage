using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Effects.Requests;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Attack.Events;
using _Logic.Gameplay.Units.Health;
using _Logic.Gameplay.Units.Health.Requests;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Attack.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackDamageCausingSystem : AbstractUpdateSystem
    {
        private Event<AttackEndEvent> _attackEndEvent;
        private Request<HealthChangeRequest> _healthChangeRequest;

        public override void OnAwake()
        {
            _attackEndEvent = World.GetEvent<AttackEndEvent>();
            _healthChangeRequest = World.GetRequest<HealthChangeRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var ent in _attackEndEvent.publishedChanges)
            {
                if (ent.AttackerEntity.IsNullOrDisposed() || !ent.AttackerEntity.Has<AttackComponent>() || !ent.AttackerEntity.Has<StatsComponent>()) 
                    continue;

                ref var attackComponent = ref ent.AttackerEntity.GetComponent<AttackComponent>();
                ref var statsComponent = ref ent.AttackerEntity.GetComponent<StatsComponent>();

                var healthChangeData = new HealthChangeData
                {
                    Type = attackComponent.AttackHealthChangeType,
                    Value = statsComponent.Value.GetCurrentValue(StatType.AttackDamage)
                };
                var healthChangeRequest = new HealthChangeRequest
                {
                    TargetEntity = ent.TargetEntity,
                    SenderEntity = ent.AttackerEntity,
                    Data = healthChangeData,
                    CreatePopup = true
                };
                _healthChangeRequest.Publish(healthChangeRequest);

                foreach (var impactType in attackComponent.ImpactTypes)
                {
                    World.GetRequest<ImpactCreationRequest>().Publish(new ImpactCreationRequest
                    {
                        Invoker = ent.AttackerEntity,
                        Position = ent.TargetEntity.GetComponent<TransformComponent>().Value.position,
                        Type = impactType
                    }, true); 
                }
            }
        }
    }
}