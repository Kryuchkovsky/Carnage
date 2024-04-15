using UnityEngine;

namespace _Logic.Gameplay.SurvivalMode
{
    public class GameplayUIContainer : MonoBehaviour
    {
        public void SetActivity(bool isActive) => gameObject.SetActive(isActive);
    }
}