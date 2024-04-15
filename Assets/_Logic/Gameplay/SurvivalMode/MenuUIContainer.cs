using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Gameplay.SurvivalMode
{
    public class MenuUIContainer : MonoBehaviour
    {
        [field: SerializeField] public Button PreviousHeroButton { get; private set; }
        [field: SerializeField] public Button NextHeroButton { get; private set; }
        [field: SerializeField] public Button StartButton { get; private set; }

        public void SetActivity(bool isActive) => gameObject.SetActive(isActive);
    }
}