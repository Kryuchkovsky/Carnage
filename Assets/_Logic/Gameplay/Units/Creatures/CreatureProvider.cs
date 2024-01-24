using _Logic.Gameplay.Units.Creatures.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units.Creatures
{
    public class CreatureProvider : UnitProvider
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new CreatureComponent
            {
                Value = this
            });
        }

        public override void OnDie()
        {
            base.OnDie();
            
            if (_navMeshAgent != null)
            {
                _navMeshAgent.ResetPath();
            }
        }
    }
}