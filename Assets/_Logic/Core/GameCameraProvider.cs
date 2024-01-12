using _Logic.Core;
using _Logic.Core.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _GameLogic.Common
{
    public class GameCameraProvider : ExtendedMonoProvider<CameraComponent>
    {
        [SerializeField] private Camera _camera;
        [SerializeField, Min(0)] private int _index;
        
        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new CameraComponent
            {
                Value = _camera
            });
            Entity.SetComponent(new IndexComponent
            {
                Value = _index
            });
        }
    }
}