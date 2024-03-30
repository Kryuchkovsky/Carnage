using _GameLogic.Extensions;
using _Logic.Core;
using _Logic.Core.Systems;
using _Logic.Gameplay.Camera.Systems;
using _Logic.Gameplay.Items.Systems;
using _Logic.Gameplay.Projectiles.Systems;
using _Logic.Gameplay.SurvivalMode.Systems;
using _Logic.Gameplay.Units.AI.Systems;
using _Logic.Gameplay.Units.Attack.Systems;
using _Logic.Gameplay.Units.Effects.Systems;
using _Logic.Gameplay.Units.Experience.Systems;
using _Logic.Gameplay.Units.Health.Systems;
using _Logic.Gameplay.Units.Movement.Systems;
using _Logic.Gameplay.Units.Spawn.Systems;
using _Logic.Gameplay.Units.Stats.Systems;
using _Logic.Gameplay.Units.Team.Systems;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.SurvivalMode
{
    public class SurvivalModeEcsBootstrapper : EcsBootstrapper
    {
        private SystemsGroup _systemsGroup;

        public override World World => World.Default;

        protected override void RegisterSystems(IObjectResolver resolver)
        {
            _systemsGroup = World.CreateSystemsGroup();
            _systemsGroup.AddSystem(new TimerProcessingSystem(), resolver);

            _systemsGroup.AddInitializer(new SurvivalModeInitializationSystem(), resolver);
            _systemsGroup.AddSystem(new EnemiesSpawnRequestSendingSystem(), resolver);
            _systemsGroup.AddSystem(new SurvivalModeEnhancementSystem(), resolver);
            
            _systemsGroup.AddSystem(new SpawnAbilityHandlingSystem(), resolver);
            
            _systemsGroup.AddSystem(new AreaImpactCreationRequestsProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new EffectAttachmentRequestsProcessingSystem(), resolver);
            
            _systemsGroup.AddSystem(new TeamDataSettingRequestsProcessingSystem(), resolver);

            _systemsGroup.AddSystem(new StatDependentComponentsSetRequestProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new StatsPanelProvidingSystem(), resolver);
            _systemsGroup.AddSystem(new StatChangeRequestProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new StatUpdateSystem(), resolver);
            
            _systemsGroup.AddSystem(new AttackTargetSearchSystem(), resolver);
            _systemsGroup.AddSystem(new AttackTargetValidationSystem(), resolver);
            _systemsGroup.AddSystem(new AttackTargetFollowingSystem(), resolver);
            _systemsGroup.AddSystem(new GlobalTargetFollowingSystem(), resolver);
            _systemsGroup.AddSystem(new UnitSightHandlingSystem(), resolver);

            _systemsGroup.AddSystem(new AttackCooldownTimeProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new AttackLaunchSystem(), resolver);
            _systemsGroup.AddSystem(new AttackFragmentationSystem(), resolver);
            _systemsGroup.AddSystem(new AttackReboundingSystem(), resolver);
            _systemsGroup.AddSystem(new AttackSplittingSystem(), resolver);
            _systemsGroup.AddSystem(new AttackAnimationCompletionReactionSystem(), resolver);
            
            _systemsGroup.AddSystem(new ProjectileCreationRequestProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new ProjectileDestinationUpdateSystem(), resolver);
            _systemsGroup.AddSystem(new ProjectileFlightProcessingSystem(), resolver);

            _systemsGroup.AddSystem(new ExperienceDropFromDeadSystem(), resolver);
            _systemsGroup.AddSystem(new ExperienceAmountChangeRequestProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new ExperienceBarProvidingSystem(), resolver);
            _systemsGroup.AddSystem(new LevelChangeRequestProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new LeveledUpUnitEnhancementSystem(), resolver);

            _systemsGroup.AddSystem(new HealthBarProvidingSystem(), resolver);
            _systemsGroup.AddSystem(new HealthRegenerationSystem(), resolver);
            _systemsGroup.AddSystem(new PeriodicHealthChangesHandlingSystem(), resolver);
            _systemsGroup.AddSystem(new HealthChangeRequestProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new HealthChangeProcessAdditionRequestProcessingSystem(), resolver);

            _systemsGroup.AddSystem(new ExperienceEssenceCreationRequestProcessingSystem(), resolver);
            _systemsGroup.AddSystem(new ItemCollectionSystem(), resolver);

            _systemsGroup.AddSystem(new PlayerUnitDestinationSystem(), resolver);
            _systemsGroup.AddSystem(new ManualMovementSystem(), resolver);
            _systemsGroup.AddSystem(new AutomaticMovementSystem(), resolver);
            _systemsGroup.AddSystem(new DestinationChangeRequestsProcessingSystem(), resolver);
            
            _systemsGroup.AddSystem(new UnitBuilderHandlingSystem(), resolver);
            _systemsGroup.AddSystem(new HeroPresenceControllingSystem(), resolver);
            _systemsGroup.AddSystem(new UnitSpawnRequestsHandlingSystem(), resolver);

            _systemsGroup.AddSystem(new GameCameraTargetGroupHandlingSystem(), resolver);

            World.AddSystemsGroup(4, _systemsGroup);
        }
        
        public void OnDestroy()
        {
            World.RemoveSystemsGroup(_systemsGroup);
        }
    }
}