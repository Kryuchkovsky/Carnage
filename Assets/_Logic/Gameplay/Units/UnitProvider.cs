using _Logic.Core;
using _Logic.Gameplay.Units.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units
{
    public abstract class UnitProvider : ExtendedMonoProvider<UnitComponent>
    {
        protected UnitModel Model;

        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new UnitComponent
            {
                Value = this
            });
        }

        public virtual void SetModel(UnitModel model)
        {
            if (Model)
            {
                Destroy(Model);
            }

            Model = model;
            model.transform.parent = transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.rotation = Quaternion.identity;
        }

        public virtual void OnAttack()
        {
            Model.PlayAttackAnimation();
        }

        public virtual void OnMove(float speed)
        {
            Model.SetMovementSpeed(speed);
        }

        public virtual void OnDamage()
        {
            Model.PlayHitAnimation();
        }

        public virtual void OnDie()
        {
            Model.PlayDeathAnimation();
            Destroy(gameObject, 3);
        }
    }
}