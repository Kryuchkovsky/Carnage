﻿using _Logic.Core.Systems;
using _Logic.Gameplay.Camera.Systems;
using _Logic.Gameplay.Units.AI.Systems;
using _Logic.Gameplay.Units.Attack.Systems;
using _Logic.Gameplay.Units.Buildings.Systems;
using _Logic.Gameplay.Units.Health.Systems;
using _Logic.Gameplay.Units.Movement.Systems;
using _Logic.Gameplay.Units.Projectiles.Systems;
using _Logic.Gameplay.Units.Spawn.Systems;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace _Logic.Gameplay
{
    public class FightInitializer : Initializer
    {
        private SystemsGroup _systemsGroup;
        
        public override void OnAwake()
        {
            _systemsGroup = World.CreateSystemsGroup();
            _systemsGroup.AddSystem(new TimerProcessingSystem());
            _systemsGroup.AddSystem(new BarrackSpawnHandlingSystem());

            _systemsGroup.AddSystem(new HeroPresenceControllingSystem());
            _systemsGroup.AddSystem(new CreatureSpawnRequestsHandlingSystem());
            
            _systemsGroup.AddSystem(new EnemySearchingSystem());
            _systemsGroup.AddSystem(new EnemyFollowingSystem());
            _systemsGroup.AddSystem(new GlobalTargetFollowingSystem());
            _systemsGroup.AddSystem(new SightHandlingSystem());
            
            _systemsGroup.AddSystem(new AttackCooldownTimeProcessingSystem());
            _systemsGroup.AddSystem(new MeleeAttackHandlingSystem());
            _systemsGroup.AddSystem(new RangeAttackHandlingSystem());
            _systemsGroup.AddSystem(new HomingProjectileCreationRequestsHandlingSystem());
            _systemsGroup.AddSystem(new ProjectileFlightProcessingSystem());
            _systemsGroup.AddSystem(new DamageRequestsProcessingSystem());

            _systemsGroup.AddSystem(new HealthBarAddingSystem());
            _systemsGroup.AddSystem(new HealthChangeRequestsProcessingSystem());

            _systemsGroup.AddSystem(new PlayerUnitDestinationSystem());
            _systemsGroup.AddSystem(new ManualMovementSystem());
            _systemsGroup.AddSystem(new AutomaticMovementSystem());
            _systemsGroup.AddSystem(new DestinationChangeRequestsProcessingSystem());

            _systemsGroup.AddSystem(new GameCameraTargetGroupHandlingSystem());

            World.AddSystemsGroup(4, _systemsGroup);
        }

        public override void Dispose()
        {
            base.Dispose();
            World.RemoveSystemsGroup(_systemsGroup);
        }
    }
}