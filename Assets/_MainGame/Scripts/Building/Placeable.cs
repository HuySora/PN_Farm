namespace FarmGame.Building
{
    using UnityEngine;

    public class Placeable : MonoBehaviour {
        [field: SerializeField] public SpriteRenderer Sprite { get; private set; }
        [field: SerializeField] public BoundsInt Area { get; private set; }

        [field: SerializeField] public bool IsMoving { get; private set; }

        private float m_DeltaX;
        private float m_DeltaY;


        public bool CanBePlaced()
        {
            Vector3Int pos = BuildManager.WorldToCell(transform.position);
            BoundsInt pointerArea = Area;
            pointerArea.position = pos;

            return BuildManager.CanBuildIn(pointerArea);
        }
        
        public bool TryToPlace()
        {
            Vector3Int pos = BuildManager.WorldToCell(transform.position);
            BoundsInt pointerArea = Area;
            pointerArea.position = pos;

            // Set the pool to true

            return BuildManager.TryBuildIn(pointerArea);
        }
    }
}

