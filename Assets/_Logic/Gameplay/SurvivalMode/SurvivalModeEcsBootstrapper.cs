using _Logic.Core;
using _Logic.Gameplay.SurvivalMode.Systems;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.SurvivalMode
{
    public class SurvivalModeEcsBootstrapper : EcsBootstrapper
    {
        public override World World => World.Default;

        protected override void RegisterSystems(IObjectResolver resolver)
        {
            AddInitializer<SurvivalModeInitializationSystem>();
            AddInitializer<HeroSelectionStageHandlingSystem>();
            AddSystem<EnemiesSpawnRequestSendingSystem>();
            AddSystem<SurvivalModeEnhancementSystem>();
        }
    }
}