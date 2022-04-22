namespace FarmGame
{
    using UnityEngine;
    using UnityEngine.EventSystems;

#if UNITY_EDITOR
    public partial class InteractManager
    {
        private void OnDrawGizmos()
        {
        }
    }

#endif

    public partial class InteractManager : SingletonBehaviour<InteractManager>
    {
        #region Static ----------------------------------------------------------------------------------------------------
        #endregion

        private Vector3 m_DragAnchor;
        private bool m_IsDragging;

        private void Update()
        {
        }
    }
}

