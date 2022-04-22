namespace FarmGame
{
    using UnityEngine;
    using UnityEngine.EventSystems;

#if UNITY_EDITOR
    public partial class CameraManager
    {
        [field: SerializeField] public float Editor_GizmosZ { get; private set; }

        private void OnDrawGizmos()
        {
            Color lastColor = Gizmos.color;
            Gizmos.color = Color.green;

            var center = new Vector3((m_MinX + m_MaxX) / 2, (m_MinY + m_MaxY) / 2, Editor_GizmosZ);
            var size = new Vector3(m_MaxX - m_MinX, m_MaxY - m_MinY);
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = lastColor;
        }
    }

#endif

    public partial class CameraManager : SingletonBehaviour<CameraManager>
    {
        public const float kZoomMultiplier = 0.01f;
        public const float kZoomStepMultiplier = 1f;

        #region Static ----------------------------------------------------------------------------------------------------
        public static Camera Main => Current.m_Camera;
        #endregion

        [SerializeField] private Camera m_Camera;

        [SerializeField] private float m_MinX;
        [SerializeField] private float m_MaxX;
        [SerializeField] private float m_MinY;
        [SerializeField] private float m_MaxY;

        [SerializeField, Range(0.01f, 10f)] private float m_ZoomMin = 0.01f;
        [SerializeField, Range(10f, 100f)] private float m_ZoomMax = 10f;
        [SerializeField, Range(1f, 10f)] private float m_ZoomSpeed = 1f;


        private Vector3 m_AnchorPos;
        private bool m_IsAnchored;

        private bool m_IsZooming;
        private float m_PrevMagnitude;
        private bool m_WasZoomingLastFrame;

        private void LateUpdate()
        {
            HandleZoom();
            HandleDrag();
        }

        private void HandleZoom()
        {
            // Touch
            if (Input.touchCount == 2)
            {
                var touchZero = Input.GetTouch(0);
                var touchOne = Input.GetTouch(1);

                // Start
                if (!m_IsZooming)
                {
                    // We only care that the first touch not clicking on UI
                    if (EventSystem.current.IsPointerOverGameObject(touchZero.fingerId)) return;

                    m_PrevMagnitude = (touchOne.position - touchZero.position).magnitude;
                    m_IsZooming = true;

                    return;
                }

                // Update
                if (m_IsZooming)
                {
                    float magnitude = (touchOne.position - touchZero.position).magnitude;
                    float deltaMagnitude = magnitude - m_PrevMagnitude;
                    m_PrevMagnitude = magnitude;

                    m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize - deltaMagnitude * m_ZoomSpeed * kZoomMultiplier, m_ZoomMin, m_ZoomMax);
                    m_WasZoomingLastFrame = true;

                    return;
                }

                m_IsAnchored = false;
                return;
            }
            if (Input.touchCount == 1)
            {
                m_WasZoomingLastFrame = false;
            }

            // Mouse
            Debug.Log(Input.mouseScrollDelta.y);

            if (Input.mouseScrollDelta.y != 0) {
                float size = m_Camera.orthographicSize - Input.mouseScrollDelta.y * m_ZoomSpeed * kZoomStepMultiplier;
                m_Camera.orthographicSize = Mathf.Clamp(size, m_ZoomMin, m_ZoomMax);
            }
        }
        private void HandleDrag()
        {
            // Anchor start
            if (PrimaryPointer.WasPressedThisFrame())
            {
                // Only anchorable if not clicking on UI
                m_IsAnchored = !PrimaryPointer.IsOverGameObject();
                if (m_IsAnchored && PrimaryPointer.TryGetPosition(out var pointerPos))
                {
                    m_AnchorPos = m_Camera.ScreenToWorldPoint(pointerPos);
                }

                return;
            }
            // Dragging (1 pointer)
            else if (PrimaryPointer.IsPressed() && m_IsAnchored && PrimaryPointer.TryGetPosition(out var pointerPos))
            {
                // Adjust the position so we don't have any offset
                var offset = m_Camera.ScreenToWorldPoint(pointerPos) - m_AnchorPos;

                float confinedMinX = m_MinX + m_Camera.orthographicSize * m_Camera.aspect;
                float confinedMaxX = m_MaxX - m_Camera.orthographicSize * m_Camera.aspect;
                float confinedMinY = m_MinY + m_Camera.orthographicSize;
                float confinexMaxY = m_MaxY - m_Camera.orthographicSize;

                float clampedX = Mathf.Clamp(m_Camera.transform.position.x - offset.x, confinedMinX, confinedMaxX);
                float clampedY = Mathf.Clamp(m_Camera.transform.position.y - offset.y, confinedMinY, confinexMaxY);

                m_Camera.transform.position = new Vector3(clampedX, clampedY, m_Camera.transform.position.z);
            }
        }
    }
}

