using FarmGame.Building;

namespace FarmGame
{
    using UnityEngine;

    public class Interactable : MonoBehaviour {
        [SerializeField] private float m_HoldDuration;

        private bool m_IsHolding;
        private float m_HoldTriggerTime;

        private void Update()
        {
            // Holding start (similar to Input.touches[0].phase == TouchPhase.Began)
            if (!m_IsHolding && Input.GetMouseButtonDown(0))
            {
                m_IsHolding = true;
                m_HoldTriggerTime = Time.unscaledTime + m_HoldDuration;
            }
            // Holding (similar to Input.touches[0].phase == TouchPhase.Moved || TouchPhase.Stationary)
            else if (m_IsHolding && Input.GetMouseButton(0))
            {
                Debug.Log("Animation");
                if (Time.unscaledTime < m_HoldTriggerTime) return;

                Debug.Log("Start");

            }
            // Holding end (similar to Input.touches[0].phase == TouchPhase.Ended)
            else if (m_IsHolding && Input.GetMouseButtonUp(0))
            {
                m_IsHolding = false;
                return;
            }
        }
    }
}

