using FarmGame.Building;

namespace FarmGame
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class Interactable : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler {
        [SerializeField] private Placeable m_Placeable;
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
        [SerializeField] private float m_HoldDuration;

        private bool m_IsHolding;
        private float m_HoldTriggerTime;
        private Tweener m_HoldTweener;

        private void OnMouseDownX()
        {
            if (m_IsHolding) return;

            m_IsHolding = true;
            m_HoldTriggerTime = Time.unscaledTime + m_HoldDuration;
            m_HoldTweener = m_SpriteRenderer.transform
                .DOScale(0f, m_HoldDuration)
                .SetAutoKill(false);
            // Unscaled time for tween
            m_HoldTweener.timeScale = 1f / Time.timeScale;
        }

        private void OnMouseDragX()
        {
            if (!m_IsHolding) return;

            // Unscaled time for tween
            m_HoldTweener.timeScale = 1f / Time.timeScale;
            if (Time.unscaledTime < m_HoldTriggerTime) return;

            CancelHolding();
            BuildManager.EnterMovingMode(m_Placeable);
        }

        private void OnMouseUpX() => CancelHolding();

        private void CancelHolding()
        {
            m_IsHolding = false;
            m_HoldTweener.Rewind();
            m_HoldTweener.Kill();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnMouseDownX();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            OnMouseDragX();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            OnMouseUpX();
        }
    }
}

