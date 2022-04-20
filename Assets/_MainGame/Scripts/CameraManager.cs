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
        #region Static ----------------------------------------------------------------------------------------------------
        public static Camera Main => Current.m_Camera;
        #endregion

        [SerializeField] private Camera m_Camera;

        [SerializeField] private float m_MinX;
        [SerializeField] private float m_MaxX;
        [SerializeField] private float m_MinY;
        [SerializeField] private float m_MaxY;
        [SerializeField] private float m_ZoomMin;
        [SerializeField] private float m_ZoomMax;


        private Vector3 m_DragAnchor;
        private bool m_CanDrag;

        private void LateUpdate()
        {
            // Dragging start (similar to Input.touches[0].phase == TouchPhase.Began)
            if (Input.GetMouseButtonDown(0))
            {
                // Only draggable if not clicking on UI
                m_CanDrag = !EventSystem.current.IsPointerOverGameObject();
                if (m_CanDrag)
                {
                    m_DragAnchor = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            // Dragging (similar to Input.touches[0].phase == TouchPhase.Moved || TouchPhase.Stationary)
            else if (Input.GetMouseButton(0) && m_CanDrag)
            {
                // Adjust the position so we don't have any offset
                var offset = m_Camera.ScreenToWorldPoint(Input.mousePosition) - m_DragAnchor;
                float clampedX = Mathf.Clamp(m_Camera.transform.position.x - offset.x, m_MinX, m_MaxX);
                float clampedY = Mathf.Clamp(m_Camera.transform.position.y - offset.y, m_MinY, m_MaxY);
                m_Camera.transform.position = new Vector3(clampedX, clampedY, m_Camera.transform.position.z);
            }
            // Zooming
            else if (Input.touchCount == 2)
            {
                //var touchZero = Input.GetTouch(0);
                //var touchOne = Input.GetTouch(1); 
            }
        }
    }
}

