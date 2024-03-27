using _Logic.Core;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Health.Components;
using _Logic.Gameplay.Units.Movement.Components;
using _Logic.Gameplay.Units.Stats.Components;
using _Logic.Gameplay.Units.Stats.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Stats.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class StatDependentComponentsSetRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<StatDependentComponentsSetRequest> _request;
        
        public override void OnAwake()
        {
            _request = World.GetRequest<StatDependentComponentsSetRequest>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _request.Consume())
            {
                if (request.Entity.IsNullOrDisposed() && request.Entity.Has<StatsComponent>() && request.Entity.Has<UnitDataComponent>()) continue;
                
                ref var statsComponent = ref request.Entity.GetComponent<StatsComponent>();
                ref var unitDataComponent = ref request.Entity.GetComponent<UnitDataComponent>();
                var stats = statsComponent.Value;
                var data = unitDataComponent.Value;

                var hasAllAttackStats = stats.HasStat(StatType.AttackDamage) && stats.HasStat(StatType.AttackRange) &&
                                        stats.HasStat(StatType.AttackSpeed) && stats.HasStat(StatType.AttackTime);

                if (hasAllAttackStats && !request.Entity.Has<AttackComponent>())
                {
                    request.Entity.SetComponent(new AttackComponent
                    {
                        ProjectileType = data.ProjectileType,
                        AttackHealthChangeType = data.AttackHealthChangeType
                    });
                }
                else if (!hasAllAttackStats && request.Entity.Has<AttackComponent>())
                {
                    request.Entity.RemoveComponent<AttackComponent>();
                }
                
                var hasAllMovementStats = stats.HasStat(StatType.MovementSpeed) && stats.HasStat(StatType.RotationSpeed);

                if (hasAllMovementStats && !request.Entity.Has<MovementComponent>())
                {
                    request.Entity.AddComponent<MovementComponent>();
                }
                else if (!hasAllMovementStats && request.Entity.Has<MovementComponent>())
                {
                    request.Entity.RemoveComponent<MovementComponent>();
                }
                
                var hasAllHealthStats = stats.HasStat(StatType.MaxHeath) && stats.HasStat(StatType.HealthRegenerationRate);

                if (hasAllHealthStats && !request.Entity.Has<HealthComponent>())
                {
                    request.Entity.SetComponent(new HealthComponent
                    {
                        Percentage = 1
                    });
                }
                else if (!hasAllAttackStats && request.Entity.Has<HealthComponent>())
                {
                    request.Entity.RemoveComponent<AttackComponent>();
                }
            }
        }
    }
}