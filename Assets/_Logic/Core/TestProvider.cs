using _Logic.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace _Logic.Core
{
    public class TestProvider : MonoProvider<TransformComponent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.AddComponent<IdComponent>();
        }
    }
}