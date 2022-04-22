namespace FarmGame.Building
{
    using UnityEngine;

#if UNITY_EDITOR
    public partial class Placeable
    {
        // TODO: OnDrawGizmos() for area visualise
        private void OnDrawGizmos()
        {
            return;
        }
    }
#endif

    /// <summary>
    /// Base class for <seealso cref="BuildManager"/> interaction.
    /// </summary>
    public partial class Placeable : MonoBehaviour {
        // PROTOTYPE: Using All1SpritesShader to create vfx
        [Header("Dependencies")]
        [SerializeField] private SpriteRenderer m_SpriteRenderer;

        [Header("Settings")]
        [Tooltip("Upper-right direction size")]
        [SerializeField, Range(1f, 10f)] private int m_SizeX;
        [Tooltip("Upper-left direction size")]
        [SerializeField, Range(1f, 10f)] private int m_SizeY;

        public bool IsRotated => m_IsRotated;
        /// <summary>
        /// Size of X of the placeable with the current rotation
        /// </summary>
        public int SizeX => m_IsRotated ? m_SizeY : m_SizeX;
        /// <summary>
        /// Size of Y of the placeable with the current rotation
        /// </summary>
        public int SizeY => m_IsRotated ? m_SizeX : m_SizeY;
        public bool IsDragging => m_IsDragging;

        protected bool m_IsRotated;
        public bool m_IsDragging;

        /// <summary>
        /// Call when <see cref="BuildManager"/> start constructing/moving this <see cref="Placeable"/>.
        /// </summary>
        public virtual void PlaceableBegin()
        {
            if (!m_SpriteRenderer.material.IsKeywordEnabled(A1SSKeyword.GHOST_ON)) return;

            m_SpriteRenderer.material.SetFloat(A1SSProperty._GhostBlend, 1f);
        }

        /// <summary>
        /// Call when <see cref="BuildManager"/> want to rotate this <see cref="Placeable"/>.
        /// </summary>
        public virtual void Rotate()
        {
            m_IsRotated = !m_IsRotated;
            
            // Hard-coded since we only have 2 rotated view (180f and 0f)
            transform.rotation = Quaternion.AngleAxis(m_IsRotated ? 180f : 0f, transform.up);
        }

        /// <summary>
        /// Calls everyframe by <see cref="BuildManager"/>.
        /// </summary>
        /// <param name="isPlaceable"></param>
        public virtual void PlaceableUpdate(bool isPlaceable)
        {
            if (!m_SpriteRenderer.material.IsKeywordEnabled(A1SSKeyword.GREYSCALE_ON)) return;

            // Hard-coded since this only use to prototype
            m_SpriteRenderer.material.SetFloat(A1SSProperty._GreyscaleBlend, isPlaceable ? 0f : 0.4f);
        }

        /// <summary>
        /// Call when <see cref="BuildManager"/> placed this <see cref="Placeable"/>.
        /// </summary>
        public virtual void PlaceableEnd()
        {
            if (!m_SpriteRenderer.material.IsKeywordEnabled(A1SSKeyword.GHOST_ON)) return;

            m_SpriteRenderer.material.SetFloat(A1SSProperty._GhostBlend, 0f);
        }
    }
}

