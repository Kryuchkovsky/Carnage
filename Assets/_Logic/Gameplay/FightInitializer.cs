using _Logic.Core.Systems;
using _Logic.Gameplay.Buildings.Systems;
using _Logic.Gameplay.Units.Systems;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;

namespace _Logic.Gameplay
{
    public class FightInitializer : Initializer
    {
        private SystemsGroup _systemsGroup;
        
        public override void OnAwake()
        {
            _systemsGroup = World.CreateSystemsGroup();
            _systemsGroup.AddSystem(new TimerProcessingSystem());
            _systemsGroup.AddSystem(new BarrackSpawnHandlingSystem());
            _systemsGroup.AddSystem(new UnitSpawnRequestsHandlingSystem());
            _systemsGroup.AddSystem(new UnitMovementSystem());

            World.AddSystemsGroup(4, _systemsGroup);
        }

        public override void Dispose()
        {
            base.Dispose();
            World.RemoveSystemsGroup(_systemsGroup);
        }
    }
}