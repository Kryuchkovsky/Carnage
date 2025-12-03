using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Logic.Gameplay.FightMode
{
    public class UIBattleStateView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _unitsCountTeam1Text;
        [SerializeField] private TextMeshProUGUI _unitsCountTeam2Text;
        [SerializeField] private Image _fillingTeam1Image;
        [SerializeField] private Image _fillingTeam2Image;

        public void SetData(int unitsCountTeam1, int unitsCountTeam2)
        {
            var totalUnitsCount = unitsCountTeam1 + unitsCountTeam2;
            _fillingTeam1Image.fillAmount = (float)unitsCountTeam1 / totalUnitsCount;
            _fillingTeam2Image.fillAmount = (float)unitsCountTeam2 / totalUnitsCount;
            _unitsCountTeam1Text.SetText(unitsCountTeam1.ToString());
            _unitsCountTeam2Text.SetText(unitsCountTeam2.ToString());
        }
    }
}