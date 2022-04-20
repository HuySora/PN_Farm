namespace FarmGame.Shop
{
    using UnityEngine;
    
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Category", menuName = "Shop/Category")]
    public partial class ShopCategoryAsset { }
#endif

    public partial class ShopCategoryAsset : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "New Category";
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}