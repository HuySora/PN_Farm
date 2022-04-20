using FarmGame.UI;

namespace FarmGame.Building
{
    using UnityEngine;
    using UnityEngine.Tilemaps;
    
    public class BuildManager : SingletonBehaviour<BuildManager>
    {
        #region Static ----------------------------------------------------------------------------------------------------
        public static Tilemap Tilemap => Current.m_Tilemap;
        public static TileBase OccupiedTile => Current.m_OccupiedTile;
        public static void EnterBuyingMode(Placeable prefab, Vector3 pos) => Current.InnerEnterBuyingMode(prefab, pos);
        public static void EnterMovingMode(Placeable placeable) => Current.InnerEnterMovingMode(placeable);
        public static Vector3Int WorldToCell(Vector3 pos) => Current.InnerWorldToCell(pos);
        public static Vector3 GetSnappedWorldPosition(Vector3 pos) => Current.InnerGetSnappedWorldPosition(pos);
        public static void Cancle() => Current.InnerCancle();
        public static void Rotate() => Current.InnerRotate();
        public static void Confirm() => Current.InnerConfirm();
        public static bool CanBuildIn(BoundsInt area) => Current.InnerCanBuildIn(area);
        public static bool TryBuildIn(BoundsInt area) => Current.InnerTryBuildIn(area);
        #endregion
        [Header("Dependencies")]
        [SerializeField] private Tilemap m_Tilemap;
        [SerializeField] private TileBase m_OccupiedTile;
        [SerializeField] private bool m_Center;


        private Placeable m_CurrentPlaceable;
        private bool m_IsBuying;

        private void Update()
        {
            if (m_CurrentPlaceable == null) return;
            if (!m_CurrentPlaceable.IsMoving) return;


            var pos = CameraManager.Main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = m_Tilemap.transform.position.z;
            m_CurrentPlaceable.transform.position = InnerGetSnappedWorldPosition(pos);
        }

        private void InnerEnterBuyingMode(Placeable prefab, Vector3 pos)
        {
            ViewManager.SwitchToBuild();
            // Cancle previous buying mode
            if (m_IsBuying) InnerCancle();
            m_IsBuying = true;
            
            var myPos = new Vector3(pos.x, pos.y, m_Tilemap.transform.position.z);
            var snappedPos = InnerGetSnappedWorldPosition(myPos);
            //Vector3 localPos = m_Grid.CellToLocalInterpolated(cellPos);

            m_CurrentPlaceable = Instantiate(prefab, snappedPos, Quaternion.identity);

            ViewManager.SwitchToBuild();
        }

        private void InnerEnterMovingMode(Placeable placeable)
        {
            ViewManager.SwitchToBuild();
            m_CurrentPlaceable = placeable;
        }

        private Vector3Int InnerWorldToCell(Vector3 pos) => m_Tilemap.WorldToCell(pos);
        private Vector3 InnerGetSnappedWorldPosition(Vector3 pos) => m_Tilemap.CellToWorld(m_Tilemap.WorldToCell(pos));

        private void InnerCancle()
        {
            if (m_IsBuying)
            {
                Destroy(m_CurrentPlaceable.gameObject);
                ViewManager.SwitchToMain();
                m_IsBuying = false;
            }
        }

        private void InnerRotate()
        {
        }

        private void InnerConfirm()
        {
        }

        private bool InnerCanBuildIn(BoundsInt area)
        {
            TileBase[] tiles = m_Tilemap.GetTilesBlock(area);

            foreach (var tile in tiles)
            {
                if (tile != null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool InnerTryBuildIn(BoundsInt area)
        {
            if (CanBuildIn(area))
            {
                var tiles = new TileBase[area.size.x * area.size.y];
                for(int i = 0; i < tiles.Length; i++)
                {
                    tiles[i] = m_OccupiedTile;
                }

                m_Tilemap.SetTilesBlock(area, tiles);
                return true;
            }

            return false;
        }
    }
}

