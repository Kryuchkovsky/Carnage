using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Levels.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Levels
{
    public class LevelProvider : GameObjectProvider<LevelComponent>
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new LevelComponent
            {
                Value = this
            });
            Entity.SetComponent(new BoundsComponent
            {
                Value = _meshRenderer.bounds
            });
        }
    }
}
