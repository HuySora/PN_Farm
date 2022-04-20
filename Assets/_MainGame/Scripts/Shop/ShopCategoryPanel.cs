namespace FarmGame.Shop
{
    using UnityEngine;
    using TMPro;

    public class ShopCategoryPanel : MonoBehaviour {
        [field: SerializeField] public TextMeshProUGUI Title { get; private set; }
        [field: SerializeField] public RectTransform ContentTransform { get; private set; }
    }
}

