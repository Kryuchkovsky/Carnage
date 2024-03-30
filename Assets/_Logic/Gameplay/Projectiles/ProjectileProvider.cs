using _Logic.Core;
using _Logic.Gameplay.Projectiles.Components;
using Scellecs.Morpeh;

namespace _Logic.Gameplay.Projectiles
{
    public class ProjectileProvider : GameObjectProvider<ProjectileComponent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new ProjectileComponent
            {
                Value = this
            });
        }
    }
}