namespace FarmGame.UI
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class MainView : ViewBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private GraphicRaycaster m_GraphicRaycaster;
        

        [Header("Settings")]
        [SerializeField] private float m_FadeDuration;

        private bool m_IsOpened;
        private Tweener m_Tweener;

        private void Awake()
        {
            m_Canvas.enabled = false;
        }

        public override void Open()
        {
            if (m_IsOpened) return;

            // OPTIMIZABLE: Reuse tweens? (1)
            m_Tweener.Kill();
            m_Canvas.enabled = true;
            m_GraphicRaycaster.enabled = true;
            m_Tweener = m_CanvasGroup.DOFade(1f, m_FadeDuration);

            m_IsOpened = true;
        }

        public override void Close()
        {
            if (!m_IsOpened) return;

            // OPTIMIZABLE: Reuse tweens? (2)
            m_Tweener.Kill();
            m_GraphicRaycaster.enabled = false;
            m_Tweener = m_CanvasGroup.DOFade(0f, m_FadeDuration)
                                     .OnComplete(() => m_Canvas.enabled = false);

            m_IsOpened = false;
        }
    }
}