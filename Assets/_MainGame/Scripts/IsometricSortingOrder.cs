namespace FarmGame
{
    using UnityEngine;
    
    public class IsometricSortingOrder : MonoBehaviour {
        [Header("Dependencies")]
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
        [SerializeField] private Transform m_TargetTransform;
        
        [Header("Settings")]
        [SerializeField, Range(1, 100)] private int m_Precision = 100;

        void Update()
        {
            // OPTIMIZABLE: Trigger box?
            m_SpriteRenderer.sortingOrder = -(int)(m_TargetTransform.position.y * m_Precision);
        }
    }
}

