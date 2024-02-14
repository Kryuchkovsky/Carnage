using _Logic.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace _Logic.Core
{
    public abstract class GameObjectProvider<T> : MonoProvider<T> where T : struct, IComponent
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new TransformComponent
            {
                Value = transform
            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!Entity.IsNullOrDisposed())
            {
                Entity.SetComponent(new ActivityComponent
                {
                    Value = true
                });
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (!Entity.IsNullOrDisposed())
            {
                Entity.SetComponent(new ActivityComponent
                {
                    Value = false
                });
            }
        }
    }
}