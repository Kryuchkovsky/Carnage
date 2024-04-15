using _Logic.Core;
using _Logic.Gameplay.Effects.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Effects
{
    public class ImpactProvider : GameObjectProvider<ImpactComponent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            
            Entity.SetComponent(new ImpactComponent()
            {
                Value = this
            });
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Entity.IsNullOrDisposed() && Entity.Has<ImpactDataComponent>())
            {
                var dataComponent = Entity.GetComponent<ImpactDataComponent>();
                var radius = dataComponent.Data.CalculateRadius(dataComponent.Progress);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, radius);
            } 
        }
#endif
    }
}