using _GameLogic.Extensions;
using _Logic.Core;
using _Logic.Core.Systems;
using _Logic.Gameplay.Abilities.Systems;
using _Logic.Gameplay.Camera.Systems;
using _Logic.Gameplay.Effects.Systems;
using _Logic.Gameplay.Items.Systems;
using _Logic.Gameplay.Projectiles.Systems;
using _Logic.Gameplay.SurvivalMode.Systems;
using _Logic.Gameplay.Units.AI.Systems;
using _Logic.Gameplay.Units.Attack.Systems;
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
        public override World World => World.Default;

        protected override void RegisterSystems(IObjectResolver resolver)
        {
            AddSystem<TimerProcessingSystem>();

            AddInitializer<SurvivalModeInitializationSystem>();
            AddInitializer<HeroSelectionStageHandlingSystem>();
            AddSystem<EnemiesSpawnRequestSendingSystem>();
            AddSystem<SurvivalModeEnhancementSystem>();
            
            AddInitializer<AbilitiesRegistrationSystem>();

            AddSystem<SpawnAbilityHandlingSystem>();
            
            AddSystem<ImpactCreationRequestsProcessingSystem>();
            AddSystem<ImpactHandlingSystem>();
            AddSystem<EffectAttachmentRequestsProcessingSystem>();
            
            AddSystem<TeamDataSettingRequestsProcessingSystem>();

            AddSystem<StatDependentComponentsSetRequestProcessingSystem>();
            AddSystem<StatsPanelProvidingSystem>();
            AddSystem<StatChangeRequestProcessingSystem>();
            AddSystem<StatUpdateSystem>();
            
            AddSystem<AttackTargetSearchSystem>();
            AddSystem<AttackTargetValidationSystem>();
            AddSystem<AttackTargetFollowingSystem>();
            AddSystem<GlobalTargetFollowingSystem>();
            AddSystem<UnitSightHandlingSystem>();

            AddSystem<AttackCooldownTimeProcessingSystem>();
            AddSystem<AttackAnimationLaunchSystem>();
            AddSystem<AttackAnimationCompletionSystem>();
            AddSystem<AttackRequestProcessingSystem>();
            AddSystem<AttackDamageCausingSystem>();
            
            AddSystem<AttackFragmentationSystem>();
            AddSystem<AttackReboundingSystem>();
            AddSystem<AttackSplittingSystem>();

            AddSystem<ProjectileCreationRequestProcessingSystem>();
            AddSystem<ProjectileDestinationUpdateSystem>();
            AddSystem<ProjectileFlightProcessingSystem>();

            AddSystem<ExperienceDropFromDeadSystem>();
            AddSystem<ExperienceAmountChangeRequestProcessingSystem>();
            AddSystem<LevelChangeRequestProcessingSystem>();
            AddSystem<ExperienceBarProvidingSystem>();
            AddSystem<ExperienceBarUpdateSystem>();
            AddSystem<LeveledUpUnitEnhancementSystem>();

            AddSystem<HealthBarProvidingSystem>();
            AddSystem<HealthRegenerationSystem>();
            AddSystem<PeriodicHealthChangesHandlingSystem>();
            AddSystem<PeriodicHealthChangesResetSystem>();
            AddSystem<HealthChangeRequestProcessingSystem>();
            AddSystem<HealthChangeProcessAdditionRequestProcessingSystem>();

            AddSystem<ExperienceEssenceCreationRequestProcessingSystem>();
            AddSystem<ItemCollectionSystem>();

            AddSystem<PlayerUnitDestinationSystem>();
            AddSystem<ManualMovementSystem>();
            AddSystem<AutomaticMovementSystem>();
            AddSystem<DestinationChangeRequestsProcessingSystem>();
            
            AddSystem<UnitBuilderHandlingSystem>();
            AddSystem<UnitSpawnRequestsHandlingSystem>();

            AddSystem<GameCameraTargetGroupHandlingSystem>();
        }
    }
}