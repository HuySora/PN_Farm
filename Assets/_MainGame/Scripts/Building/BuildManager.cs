using FarmGame.UI;

namespace FarmGame.Building
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Tilemaps;

    public class BuildManager : SingletonBehaviour<BuildManager>
    {
        #region Static ----------------------------------------------------------------------------------------------------
        public static void EnterConstructingMode(Placeable prefab, Vector3 pos) => Current.InnerEnterConstructingMode(prefab, pos);
        public static void EnterMovingMode(Placeable placeable) => Current.InnerEnterMovingMode(placeable);
        public static void Cancle() => Current.InnerCancle();
        public static void Rotate() => Current.InnerRotate();
        public static void Confirm() => Current.InnerConfirm();
        #endregion

        [Header("Dependencies")]
        [SerializeField] private Tilemap m_Tilemap;
        [SerializeField] private TileBase m_OccupiedTile;

        private bool m_IsConstructing;
        private Placeable m_CurrentPlaceable;

        private void Update()
        {
            if (m_CurrentPlaceable == null) return;

            Vector2 pos;
            Vector3Int cellPos;
            if (m_CurrentPlaceable.IsDragging && PrimaryPointer.IsOverGameObject())
            {
                pos = CameraManager.Main.ScreenToWorldPoint(Input.mousePosition);
                cellPos = WorldToCell(pos);
                // Snap the placeable position to the cell
                m_CurrentPlaceable.transform.position = CellToWorld(cellPos);
            }
            else
            {
                pos = m_CurrentPlaceable.transform.position;
                cellPos = WorldToCell(pos);
            }

            // Update placeable
            m_CurrentPlaceable.PlaceableUpdate(CanTakeTilemapArea(cellPos, m_CurrentPlaceable.SizeX, m_CurrentPlaceable.SizeY));
        }

        private void InnerEnterConstructingMode(Placeable prefab, Vector3 pos)
        {
            if (m_IsConstructing) InnerCancle();
            m_IsConstructing = true;

            // Instantiate at snapped world coordinate
            var snappedPos = WorldToSnappedWorld(pos);
            m_CurrentPlaceable = Instantiate(prefab, snappedPos, Quaternion.identity);

            // Trigger begin & switch view
            m_CurrentPlaceable.PlaceableBegin();
            ViewManager.SwitchTo<BuildView>();
        }
        private void InnerEnterMovingMode(Placeable placeable)
        {
            // Clear occupied area on tilemap at selected placeable
            m_CurrentPlaceable = placeable;
            var cellPos = WorldToCell(m_CurrentPlaceable.transform.position);
            ClearTilemapArea(cellPos, m_CurrentPlaceable.SizeX, m_CurrentPlaceable.SizeY);

            // Trigger begin & switch view
            m_CurrentPlaceable.PlaceableBegin();
            ViewManager.SwitchTo<BuildView>();
        }

        private void InnerCancle()
        {
            if (m_CurrentPlaceable == null) return;

            m_CurrentPlaceable.PlaceableEnd();

            // Destroy the instance since we 
            if (m_IsConstructing)
            {
                Destroy(m_CurrentPlaceable.gameObject);
                m_IsConstructing = false;
            }
            m_CurrentPlaceable = null;

            ViewManager.SwitchTo<MainView>();
        }
        private void InnerRotate()
        {
            if (m_CurrentPlaceable != null) m_CurrentPlaceable.Rotate();
        }
        private void InnerConfirm()
        {
            if (m_CurrentPlaceable == null) return;

            var cellPos = WorldToCell(m_CurrentPlaceable.transform.position);
            if (!CanTakeTilemapArea(cellPos, m_CurrentPlaceable.SizeX, m_CurrentPlaceable.SizeY)) return;
            
            TakeTilemapArea(cellPos, m_CurrentPlaceable.SizeX, m_CurrentPlaceable.SizeY);
            m_CurrentPlaceable.PlaceableEnd();
            m_CurrentPlaceable = null;
            ViewManager.SwitchTo<MainView>();
        }

        /// <summary>
        /// World coordinate to cell coordinate of <see cref="m_Tilemap"/>.
        /// </summary>
        private Vector3Int WorldToCell(Vector3 pos) => m_Tilemap.WorldToCell(new Vector3(pos.x, pos.y, m_Tilemap.transform.position.z));
        /// <summary>
        /// Cell coordinate of <see cref="m_Tilemap"/> to world coordinate.
        /// </summary>
        private Vector3 CellToWorld(Vector3Int pos) => m_Tilemap.CellToWorld(pos);
        /// <summary>
        /// World coordinate to snapped world coordinate of <see cref="m_Tilemap"/>.
        /// </summary>
        private Vector3 WorldToSnappedWorld(Vector3 pos) => m_Tilemap.CellToWorld(WorldToCell(pos));

        
        private bool CanTakeTilemapArea(Vector3Int pos, int sizeX, int sizeY)
        {
            // Occupied area (sizeZ = 1 as we only use flat land)
            var area = new BoundsInt(pos.x, pos.y, pos.z, sizeX, sizeY, 1);
            return IsTilemapAreaOccupied(area);
        }
        private bool IsTilemapAreaOccupied(BoundsInt area)
        {
            foreach (var tile in m_Tilemap.GetTilesBlock(area))
            {
                if (tile != null) return false;
            }

            return true;
        }
        private void TakeTilemapArea(Vector3Int pos, int sizeX, int sizeY)
        {
            // Occupied area (sizeZ = 1 as we only use flat land)
            var area = new BoundsInt(pos.x, pos.y, pos.z, sizeX, sizeY, 1);
            m_Tilemap.SetTilesBlock(area, m_OccupiedTile.CreateTileArray(area));
        }
        private void ClearTilemapArea(Vector3Int pos, int sizeX, int sizeY)
        {
            // Occupied area (sizeZ = 1 as we only use flat land)
            var area = new BoundsInt(pos.x, pos.y, pos.z, sizeX, sizeY, 1);
            m_Tilemap.SetTilesBlock(area, Extension.CreateTileArray(null, area));
        }
    }
}

