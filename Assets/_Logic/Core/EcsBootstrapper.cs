using System;
using _Logic.Core.Systems;
using _Logic.Gameplay;
using _Logic.Gameplay.Abilities.Systems;
using _Logic.Gameplay.Camera.Systems;
using _Logic.Gameplay.Effects.Systems;
using _Logic.Gameplay.Items.Systems;
using _Logic.Gameplay.Projectiles.Systems;
using _Logic.Gameplay.Testing.Systems;
using _Logic.Gameplay.Units.AI.Systems;
using _Logic.Gameplay.Units.Attack.Systems;
using _Logic.Gameplay.Units.Experience.Systems;
using _Logic.Gameplay.Units.Health.Systems;
using _Logic.Gameplay.Units.Movement.Systems;
using _Logic.Gameplay.Units.Spawn.Systems;
using _Logic.Gameplay.Units.Stats.Systems;
using _Logic.Gameplay.Units.Team.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using VContainer;

namespace _Logic.Core
{
    public abstract class EcsBootstrapper : MonoBehaviour
    {
        [SerializeField] private GameLifetimeScope _lifetimeScope;
        [SerializeField] private int _order;
        
        private SystemsGroup _systemsGroup;

        public World World { get; private set; }

        private bool _isRegistered;

        private void Awake()
        {
            WorldExtensions.InitializationDefaultWorld();
            World = World.Default;
            
            _systemsGroup = World.CreateSystemsGroup();

            RegisterCommonSystems(_lifetimeScope.Container);
            RegisterSystems(_lifetimeScope.Container);
            _isRegistered = true;
            
            World.AddSystemsGroup(_order, _systemsGroup);
        }
        
        public EcsBootstrapper AddInitializer<T>() where T : class, IInitializer, new()
        {
            var instance = new T() as IInitializer;
            _lifetimeScope.Container.Inject(instance);
            _systemsGroup.AddInitializer(instance);
            
            return this;
        }

        public EcsBootstrapper AddInitializer(IInitializer instance)
        {
            _lifetimeScope.Container.Inject(instance);
            _systemsGroup.AddInitializer(instance);
            
            return this;
        }
        
        public EcsBootstrapper AddSystem<T>() where T : class, ISystem, new()
        {
            var instance = new T() as ISystem;
            _lifetimeScope.Container.Inject(instance);
            _systemsGroup.AddSystem(instance);

            return this;
        }
        
        public EcsBootstrapper AddSystem(ISystem system)
        {
            _lifetimeScope.Container.Inject(system);
            _systemsGroup.AddSystem(system);

            return this;
        }

        private void RegisterCommonSystems(IObjectResolver resolver)
        {
            AddSystem<TimerProcessingSystem>();

#if UNITY_EDITOR
            AddInitializer<TestingCommandsHandlingSystem>();
#endif
            
            AddInitializer<AbilitiesRegistrationSystem>();
            AddSystem<SpawnAbilityHandlingSystem>();
            
            AddSystem<ImpactCreationRequestsProcessingSystem>();
            AddSystem<ImpactHandlingSystem>();
            AddSystem<EffectAttachmentRequestsProcessingSystem>();
            
            AddSystem<TeamDataSettingRequestsProcessingSystem>();

            AddSystem<StatDependentComponentsSetRequestProcessingSystem>();
            AddInitializer<StatsPanelProvidingSystem>();
            AddSystem<StatChangeRequestProcessingSystem>();
            AddSystem<StatUpdateSystem>();

            AddSystem<AttackTargetValidationSystem>();
            AddSystem<AttackTargetSearchSystem>();
            AddSystem<AttackTargetFollowingSystem>();
            //AddSystem<GlobalTargetFollowingSystem>();
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

        protected abstract void RegisterSystems(IObjectResolver resolver);

        private void OnDestroy()
        {
            World.Dispose();
        }
    }
}