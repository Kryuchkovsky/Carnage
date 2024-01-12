using _Logic.Core.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace _Logic.Core
{
    public abstract class ExtendedMonoProvider<T> : MonoProvider<T> where T : struct, IComponent
    {
        [field: SerializeField] public string Id { get; private set; }
        
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new TransformComponent
            {
                Value = transform
            });
        }
    }
}