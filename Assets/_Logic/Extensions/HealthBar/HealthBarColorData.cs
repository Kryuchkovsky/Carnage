using System;
using UnityEngine;

namespace _Logic.Extensions.HealthBar
{
    [Serializable]
    public class HealthBarColorData
    {
        [field: SerializeField] public Color32 DamageColor { get; private set; }
        [field: SerializeField] public Color32 HealingColor { get; private set; }
        [field: SerializeField] public Color32 BaseColor { get; private set; }

        public HealthBarColorData(Color32 damageColor, Color32 healingColor, Color32 baseColor)
        {
            DamageColor = damageColor;
            HealingColor = healingColor;
            BaseColor = baseColor;
        }
    }
}