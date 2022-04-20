using FarmGame.Building;
using FarmGame.UI;

namespace FarmGame.Shop
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ShopExit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {
        public void OnPointerClick(PointerEventData eventData)  => ViewManager.SwitchToMain();
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!eventData.dragging) return;
            if (!eventData.pointerDrag.TryGetComponent(out ShopItemDraggable draggable)) return;
            if (draggable.ItemHolder.ItemAsset.Prefab == null) return;

            var pos = CameraManager.Main.ScreenToWorldPoint(eventData.position);
            BuildManager.EnterBuyingMode(draggable.ItemHolder.ItemAsset.Prefab, pos);
        }
    }
}

