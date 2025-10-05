using UnityEngine;
using UnityEngine.Tilemaps;

namespace DrillGame.Managers
{
    public class GridManager : MonoBehaviour
    {
        #region Fields & Properties
        [SerializeField]    
        private Tilemap previewTilemap;

        [SerializeField]
        private TileBase occupiedTile;

        private Grid grid;

        #endregion

        #region Singleton & initialization
        private void Initialize()
        {
            grid = GetComponent<Grid>();
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public Vector3Int WorldToCell(Vector3 worldPosition)
        {
            return grid.WorldToCell(worldPosition);
        }

        public Vector3 CellToWorld(Vector3Int cellPosition)
        {
            return grid.CellToWorld(cellPosition);
        }

        public bool IsCellOccupied(Vector3Int cellPosition)
        {
            return previewTilemap.GetTile(cellPosition) == occupiedTile;
        }

        public void SetCellOccupied(Vector3Int cellPosition, bool occupied)
        {
            if (occupied)
            {
                previewTilemap.SetTile(cellPosition, occupiedTile);
            }
            else
            {
                previewTilemap.SetTile(cellPosition, null);
            }
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {
            Initialize();
        }
        #endregion
    }
}
