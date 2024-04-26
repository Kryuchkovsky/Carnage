using _Logic.Core.Components;
using Cinemachine;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Core
{
    public class GameCameraProvider : GameObjectProvider<GameCameraComponent>
    {
        [SerializeField] private CinemachineVirtualCamera _fightVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera _menuVirtualCamera;
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

        public void SetFightCamera()
        {
            _fightVirtualCamera.Priority = 1000;
            _menuVirtualCamera.Priority = 0;
        }
        
        public void SetMenuCamera()
        {
            _fightVirtualCamera.Priority = 0;
            _menuVirtualCamera.Priority = 1000;
        }
    }
}