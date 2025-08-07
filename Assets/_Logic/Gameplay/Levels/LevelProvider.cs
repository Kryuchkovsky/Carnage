using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Gameplay.Levels.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Levels
{
    public abstract class LevelProvider : GameObjectProvider<LevelComponent>
    {
        [SerializeField] private Collider _collider;
        
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new LevelComponent
            {
                Value = this
            });
            Entity.SetComponent(new BoundsComponent
            {
                Value = _collider.bounds
            });
        }
    }
}