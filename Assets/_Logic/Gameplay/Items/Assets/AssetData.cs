using _Logic.Extensions.Configs;
using UnityEngine;

namespace _Logic.Gameplay.Items.Assets
{
    [CreateAssetMenu(menuName = nameof(AssetData), fileName = nameof(AssetData), order = 0)]
    public class AssetData : Data<AssetType>
    {
        [field: SerializeField] public AssetProvider Prefab { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}