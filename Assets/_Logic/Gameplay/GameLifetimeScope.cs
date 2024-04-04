using _Logic.Extensions.Configs;
using _Logic.Extensions.HealthBar;
using _Logic.Extensions.Input;
using _Logic.Extensions.Popup;
using _Logic.Extensions.VFXManager;
using _Logic.Gameplay.Effects;
using _Logic.Gameplay.Items;
using _Logic.Gameplay.Projectiles;
using _Logic.Gameplay.Rewards;
using _Logic.Gameplay.SurvivalMode;
using _Logic.Gameplay.Units;
using _Logic.Gameplay.Units.AI;
using _Logic.Gameplay.Units.Experience;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Logic.Gameplay
{
    [DefaultExecutionOrder(-5000)]
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ConfigManager _configManager;
        [SerializeField] private InputService _inputService;
        [SerializeField] private HealthBarsService _healthBarsService;
        [SerializeField] private PopupsService _popupsService;
        [SerializeField] private RewardSelector _rewardSelector;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_inputService);
            builder.RegisterInstance(_popupsService);
            builder.RegisterInstance(_rewardSelector);
            
            builder.RegisterInstance(_configManager);
            builder.RegisterInstance(_configManager.GetConfig<AISettings>());
            builder.RegisterInstance(_configManager.GetConfig<ExperienceSettings>());
            builder.RegisterInstance(_configManager.GetConfig<GameEffectCatalog>());
            builder.RegisterInstance(_configManager.GetConfig<ImpactCatalog>());
            builder.RegisterInstance(_configManager.GetConfig<ProjectilesCatalog>());
            builder.RegisterInstance(_configManager.GetConfig<SurvivalModeSettings>());
            builder.RegisterInstance(_configManager.GetConfig<UnitsCatalog>());
            builder.RegisterInstance(_configManager.GetConfig<ItemConfig>());
            
            var vfxConfig = _configManager.GetConfig<VFXCatalog>();
            var vfxService = new VFXService(vfxConfig);
            builder.RegisterInstance(vfxService);
            builder.RegisterInstance(vfxConfig);

            var healthBarSettings = _configManager.GetConfig<HealthBarSettings>();
            _healthBarsService.Initialize(healthBarSettings);
            builder.RegisterInstance(_healthBarsService);
            builder.RegisterInstance(healthBarSettings);
        }
    }
}