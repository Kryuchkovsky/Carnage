using _Logic.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace _Logic.Core
{
    public abstract class GameObjectProvider<T> : MonoProvider<T> where T : struct, IComponent
    {
        private Stash<TransformComponent> transformStash;
        private Stash<ActivityComponent> activityStash;

        protected World World => World.Default;
        
        protected override void Initialize()
        {
            base.Initialize();
            transformStash = World.GetStash<TransformComponent>();
            activityStash = World.GetStash<ActivityComponent>();
            transformStash.Set(Entity, new TransformComponent
            {
                Value = transform
            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!World.IsDisposed(Entity))
            {
                activityStash.Set(Entity, new ActivityComponent
                {
                    Value = true
                });
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (!World.IsDisposed(Entity))
            {
                activityStash.Set(Entity, new ActivityComponent
                {
                    Value = false
                });
            }
        }
    }
}