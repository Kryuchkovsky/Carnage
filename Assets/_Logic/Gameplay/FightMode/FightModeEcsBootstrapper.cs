using _Logic.Core;
using _Logic.Gameplay.FightMode.Systems;
using VContainer;

namespace _Logic.Gameplay.FightMode
{
    public class FightModeEcsBootstrapper : EcsBootstrapper
    {
        protected override void RegisterSystems(IObjectResolver resolver)
        {
            AddInitializer<FightModeInitializationSystem>();
            AddInitializer<FightSelectionStageHandlingSystem>();
        }
    }
}