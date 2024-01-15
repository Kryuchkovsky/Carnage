using _Logic.Core;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitProvider : ExtendedMonoProvider<UnitComponent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new UnitComponent
            {
                Value = this
            });
        }

        public virtual void Kill()
        {
            Destroy(this);
        }

        public virtual void PlayAttackAnimation()
        {
        }
        
        public virtual void SetMovementSpeed(float value)
        {
        }
    }
}