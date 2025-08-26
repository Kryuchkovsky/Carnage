using _GameLogic.Core;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace _Logic.Gameplay.Testing.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class TestingCommandsHandlingSystem : AbstractInitializer
    {
        private InputActionAsset _actionAsset;
        private InputAction _reloadSceneAction;

        private const string RELOAD_SCENE = "Reload scene";
        
        public override void OnAwake()
        {
            if (EventSystem.current.TryGetComponent<InputSystemUIInputModule>(out var module))
            {
                _actionAsset = module.actionsAsset;
                _reloadSceneAction = module.actionsAsset.FindAction(RELOAD_SCENE);
                _reloadSceneAction.performed += OnReloadSceneActionPerformed;
            }
        }

        private void OnReloadSceneActionPerformed(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public override void Dispose()
        {
            if (_reloadSceneAction is not null)
                _reloadSceneAction.performed -= OnReloadSceneActionPerformed;
        }
    }
}