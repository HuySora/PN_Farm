using FarmGame.Building;
using FarmGame.Economy;

namespace FarmGame.Shop
{
    using UnityEngine;

#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Item", menuName = "Shop/Item")]
    public partial class ShopItemAsset { }
#endif
    
    public partial class ShopItemAsset : ScriptableObject {
        [field: SerializeField] public string Name { get; private set; } = "New Item";
        [field: SerializeField] public string Description { get; private set; } = "Default";
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public ShopCategoryAsset ShopCategoryAsset { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public CurrencyAsset CurrencyAsset { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public Placeable Prefab { get; private set; }
    }
}

