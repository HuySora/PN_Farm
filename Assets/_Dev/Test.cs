namespace FarmGame
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class Test : MonoBehaviour {
        [SerializeField] private Grid m_Grid;
        [SerializeField] private Tilemap m_Tilemap;

        private Vector3 m_WorldPosition;


        private void Update()
        {
            m_WorldPosition = CameraManager.Main.ScreenToWorldPoint(Input.mousePosition);

            var gridCellPos = m_Grid.WorldToCell(new Vector3(m_WorldPosition.x, m_WorldPosition.y, m_Grid.transform.position.z));
            var tileCellPos = m_Tilemap.WorldToCell(new Vector3(m_WorldPosition.x, m_WorldPosition.y, m_Tilemap.transform.position.z));
;

            Debug.Log($"W: {m_WorldPosition}, G: {gridCellPos}, T: {tileCellPos}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(m_WorldPosition, 1f);
        }
    }
}

