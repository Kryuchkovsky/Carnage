using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Items.Assets
{
    [CreateAssetMenu(menuName = nameof(AssetsCatalog), fileName = nameof(AssetsCatalog), order = 0)]
    public class AssetsCatalog : FunctionalConfig<AssetType, AssetData>
    {
        
    }
}