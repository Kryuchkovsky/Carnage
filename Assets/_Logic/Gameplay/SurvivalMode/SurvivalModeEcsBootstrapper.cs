using _GameLogic.Core;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.SurvivalMode
{
    public class SurvivalModeEcsBootstrapper : EcsBootstrapper
    {
        public override World World => World.Default;

        protected override void RegisterSystems()
        {
            AddInitializer<SurvivalModeInitializer>();
        }
    }
}