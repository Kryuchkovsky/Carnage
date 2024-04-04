using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Effects
{
    [CreateAssetMenu(menuName = "Effects/Create GameEffectCatalog", fileName = "GameEffectCatalog", order = 0)]
    public class GameEffectCatalog : FunctionalConfig<EffectType, EffectData>
    {
    }
}