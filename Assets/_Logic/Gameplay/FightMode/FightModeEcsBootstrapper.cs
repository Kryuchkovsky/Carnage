using _Logic.Core;
using _Logic.Gameplay.FightMode.Systems;
using Scellecs.Morpeh;
using VContainer;

namespace _Logic.Gameplay.FightMode
{
    public class FightModeEcsBootstrapper : EcsBootstrapper
    {
        public override World World => World.Default;

        protected override void RegisterSystems(IObjectResolver resolver)
        {
            AddInitializer<FightModeInitializationSystem>();
            AddInitializer<FightSelectionStageHandlingSystem>();
        }
    }
}