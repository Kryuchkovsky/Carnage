using _Logic.Core.Components;
using Cinemachine;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Core
{
    public class GameCameraProvider : GameObjectProvider<GameCameraComponent>
    {
        [SerializeField, Min(0)] private int _index;
        
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public CinemachineTargetGroup TargetGroup { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new GameCameraComponent
            {
                Value = this
            });
            Entity.SetComponent(new IndexComponent
            {
                Value = _index
            });
        }
    }
}