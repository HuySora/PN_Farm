using FarmGame.UI;

namespace FarmGame.Shop
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ShopItemDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }
        [field: SerializeField] public ShopItemHolder ItemHolder { get; private set; }

        private Vector3 m_OriginPos;
        
        public void Inject(ShopItemHolder holder)
        {
            // NULLCHECK: Game designer error
            if (holder.TryNullCheckAndLog("Trying to injecting null.", this)) return;
            if (holder.ItemAsset.Icon.TryNullCheckAndLog("Icon is null.", holder.ItemAsset)) return;

            ItemHolder = holder;
            Image.sprite = holder.ItemAsset.Icon;
        }

        public void ResetState()
        {
            CanvasGroup.blocksRaycasts = true;
            Image.maskable = true;
            RectTransform.anchoredPosition = m_OriginPos;
        }

        private void Awake()
        {
            m_OriginPos = RectTransform.anchoredPosition;
        }

        public void OnEnable()
        {
            // OPTIMIZABLE: I think?
            ResetState();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = false;
            Image.maskable = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / ViewManager.Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetState();
        }
    }
}

