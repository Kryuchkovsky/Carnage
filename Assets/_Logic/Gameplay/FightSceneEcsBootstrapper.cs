using _GameLogic.Core;
using Scellecs.Morpeh;

namespace _Logic.Gameplay
{
    public class FightSceneEcsBootstrapper : EcsBootstrapper
    {
        public override World World => World.Default;

        protected override void RegisterSystems()
        {
            AddInitializer<FightInitializer>();
        }
    }
}