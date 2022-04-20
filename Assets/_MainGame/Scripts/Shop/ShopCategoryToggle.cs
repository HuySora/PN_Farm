namespace FarmGame.Shop
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class ShopCategoryToggle : MonoBehaviour
    {
        [field: SerializeField] public Toggle Toggle { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Title { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
    }
}