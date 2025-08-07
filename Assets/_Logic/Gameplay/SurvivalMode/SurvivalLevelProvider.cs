using _Logic.Gameplay.Levels;
using _Logic.Gameplay.SurvivalMode.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.SurvivalMode
{
    public class SurvivalLevelProvider : LevelProvider
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new SurvivalLevelComponent
            {
                Value = this
            });
        }
    }
}
