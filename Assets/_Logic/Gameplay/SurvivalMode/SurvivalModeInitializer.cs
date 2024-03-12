using _Logic.Core.Systems;
using _Logic.Gameplay.Camera.Systems;
using _Logic.Gameplay.SurvivalMode.Systems;
using _Logic.Gameplay.Units.AI.Systems;
using _Logic.Gameplay.Units.Attack.Systems;
using _Logic.Gameplay.Units.Experience.Systems;
using _Logic.Gameplay.Units.Health.Systems;
using _Logic.Gameplay.Units.Movement.Systems;
using _Logic.Gameplay.Units.Projectiles.Systems;
using _Logic.Gameplay.Units.Spawn.Systems;
using _Logic.Gameplay.Units.Team.Systems;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace _Logic.Gameplay.SurvivalMode
{
    public class SurvivalModeInitializer : Initializer
    {
        private SystemsGroup _systemsGroup;
        
        public override void OnAwake()
        {
            _systemsGroup = World.CreateSystemsGroup();
            _systemsGroup.AddSystem(new TimerProcessingSystem());

            _systemsGroup.AddInitializer(new SurvivalModeInitializationSystem());
            _systemsGroup.AddSystem(new EnemiesSpawnRequestSendingSystem());
            
            _systemsGroup.AddSystem(new SpawnAbilityHandlingSystem());

            _systemsGroup.AddSystem(new UnitBuilderHandlingSystem());
            _systemsGroup.AddSystem(new HeroPresenceControllingSystem());
            _systemsGroup.AddSystem(new UnitSpawnRequestsHandlingSystem());
            
            _systemsGroup.AddSystem(new TeamDataSettingRequestsProcessingSystem());

            _systemsGroup.AddSystem(new AttackTargetSearchSystem());
            _systemsGroup.AddSystem(new AttackTargetValidationSystem());
            _systemsGroup.AddSystem(new AttackTargetFollowingSystem());
            _systemsGroup.AddSystem(new GlobalTargetFollowingSystem());
            _systemsGroup.AddSystem(new UnitSightHandlingSystem());

            _systemsGroup.AddSystem(new AttackCooldownTimeProcessingSystem());
            _systemsGroup.AddSystem(new AttackLaunchSystem());
            _systemsGroup.AddSystem(new HomingProjectileCreationRequestsHandlingSystem());
            _systemsGroup.AddSystem(new ProjectileFlightProcessingSystem());
            _systemsGroup.AddSystem(new AttackAnimationCompletionEventsProcessingSystem());
            _systemsGroup.AddSystem(new DamageRequestsProcessingSystem());

            _systemsGroup.AddSystem(new ExperienceAmountChangeRequestProcessingSystem());
            _systemsGroup.AddSystem(new LevelChangeRequestProcessingSystem());

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