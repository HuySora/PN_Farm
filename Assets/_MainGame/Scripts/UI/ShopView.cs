namespace FarmGame.UI
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    public partial class ShopView
    {
        [field: SerializeField] public bool Editor_XOffsetByPanelWidth { get; private set; }

        private void OnValidate()
        {
            if (Editor_XOffsetByPanelWidth && m_Panel != null) m_PanelXOffset = -m_Panel.rect.width;
        }
    }
#endif

    public partial class ShopView : ViewBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private GraphicRaycaster m_GraphicRaycaster;

        [SerializeField] private RectTransform m_Panel;

        [Header("Settings")]
        [SerializeField] private float m_PanelXOffset;
        [SerializeField] private float m_SlideDuration;

        private bool m_IsOpened;
        private Tweener m_Tweener;

        private void Awake()
        {
            // Setup for tweening
            m_Canvas.enabled = false;
            var newAnchorPos = m_Panel.anchoredPosition;
            newAnchorPos.x += m_PanelXOffset;
            m_Panel.anchoredPosition = newAnchorPos;
        }

        public override void Open()
        {
            if (m_IsOpened) return;

            // OPTIMIZABLE: Reuse tweens? (1)
            m_Tweener.Kill();
            m_Canvas.enabled = true;
            m_GraphicRaycaster.enabled = true;
            m_Tweener = m_Panel.DOAnchorPosX(0, m_SlideDuration).SetEase(Ease.OutSine);

            m_IsOpened = true;
        }

        public override void Close()
        {
            if (!m_IsOpened) return;

            // OPTIMIZABLE: Reuse tweens? (2)
            m_Tweener.Kill();
            m_GraphicRaycaster.enabled = false;
            m_Tweener = m_Panel.DOAnchorPosX(m_PanelXOffset, m_SlideDuration).SetEase(Ease.OutSine)
                               .OnComplete(() => m_Canvas.enabled = false);

            m_IsOpened = false;
        }
    }
}