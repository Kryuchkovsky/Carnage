using _Logic.Extensions.Configs;
using _Logic.Gameplay.Units.Impacts;
using UnityEngine;

namespace _Logic.Gameplay.Units.Effects.Actions
{
    [CreateAssetMenu(menuName = "Impacts/Effects/Create GameEffectCatalog", fileName = "GameEffectCatalog", order = 0)]
    public class GameEffectCatalog : FunctionalConfig<EffectType, Effect>
    {
    }
}