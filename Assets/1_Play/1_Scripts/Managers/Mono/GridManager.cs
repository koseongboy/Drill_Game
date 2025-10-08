using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace DrillGame.Managers
{
    public enum TilemapType
    {
        Engine,
        Facility,
        Preview,
    }

    public class GridManager : MonoBehaviour
    {
        #region Fields & Properties
        [SerializeField]
        private Tilemap EngineTileamp;
        [SerializeField] 
        private Tilemap FacilityTileamp;
        [SerializeField]    
        private Tilemap previewTilemap;

        [SerializeField]
        private TileBase ableTile;

        [SerializeField]
        private TileBase TempFacility;

        private float distanceToCamera;

        private Grid grid;

        private Vector3Int previousCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);


        [ReadOnly, SerializeField]
        private Vector3 mouseWorldPosition;
        public bool isBatchMode { get; set; } = false;
        public TilemapType tilemapType { get; set; } = TilemapType.Preview; 

        #endregion

        #region Singleton & initialization
        private void Initialize()
        {
            distanceToCamera = -Camera.main.transform.position.z;
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
            return previewTilemap.GetTile(cellPosition) == ableTile;
        }

        public void SetCellOccupied(Vector3Int cellPosition, bool occupied)
        {
            if (occupied)
            {
                previewTilemap.SetTile(cellPosition, ableTile);
            }
            else
            {
                previewTilemap.SetTile(cellPosition, null);
            }
        }

        public void TryBatch()
        {
            FacilityTileamp.SetTile(GetCellPosition(), TempFacility);

        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (!isBatchMode) return;


            // 배치 모드일 때의 미리보기입니다
            Vector3Int cellPosition = GetCellPosition();

            if (cellPosition != previousCellPosition)
            {
                SetNullTile(previousCellPosition);
                SetPreviewTile(mouseWorldPosition);

                previousCellPosition = cellPosition;
            }

        }

        private Vector3Int GetCellPosition()
        {
            mouseWorldPosition = Mouse.current.position.ReadValue();
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, distanceToCamera)
            );
            Vector3Int cellPosition = WorldToCell(mouseWorldPosition);

            return cellPosition;
        }
        private void SetPreviewTile(Vector3 mousePosition)
        {
            Vector3Int cellPosition = WorldToCell(mousePosition);
            if (IsCellOccupied(cellPosition)) return;
            previewTilemap.SetTile(cellPosition, ableTile);
        }

        private void SetNullTile(Vector3Int cellPosition)
        {
            previewTilemap.SetTile(cellPosition, null);
        }

        #endregion
    }
}
