namespace FarmGame.Shop
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class ShopItemHolder : MonoBehaviour {
        [field: SerializeField] public TextMeshProUGUI Name { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AvailabilityText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Description { get; private set; }
        [field: SerializeField] public ShopItemDraggable ItemDrag { get; private set; }
        [field: SerializeField] public TextMeshProUGUI RequirementText { get; private set; }
        [field: SerializeField] public Image RequirementImage { get; private set; }

        [field: SerializeField] public ShopItemAsset ItemAsset { get; private set; }

        public void Inject(ShopItemAsset asset)
        {
            // NULLCHECK: Game designer error
            if (asset.TryNullCheckAndLog("Trying to injecting null.", this, this)) return;
            if (asset.CurrencyAsset.TryNullCheckAndLog("CurrencyAsset is null.", asset, this)) return;
            if (asset.CurrencyAsset.Sprite.TryNullCheckAndLog("Sprite is null", asset.CurrencyAsset, this)) return;

            ItemAsset = asset;
            Name.text = asset.Name;
            AvailabilityText.text = "N/A";
            Description.text = asset.Description;
            // TODO: Level System
            RequirementText.text = asset.Price.ToString();
            RequirementImage.sprite = asset.CurrencyAsset.Sprite;

            ItemDrag.Inject(this);
        }

    }
}

