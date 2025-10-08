using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace DrillGame.Managers
{
    public enum TilemapType
    {
        Engine,
        Facility,
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
        private Tilemap imageTilemap;

        [SerializeField]
        private TileBase unableTile;
        [SerializeField]
        private TileBase ableTile;

        [SerializeField]
        private TileBase TempEngineImage;
        [SerializeField]
        private TileBase TempFacilityImage;

        private float distanceToCamera;

        private Grid grid;

        private Vector3Int previousCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);


        [ReadOnly, SerializeField]
        private Vector3 mouseWorldPosition;
        [ReadOnly, SerializeField]
        private List<Vector3Int> entityFormation;

        private bool isBatchMode;
        private TilemapType tilemapType;
        private TileBase imageTile;

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
            //FacilityTileamp.SetTile(GetCellPosition(), TempFacilityImage);

        }

        public void EnterBatchMode(TilemapType type)
        {
            tilemapType = type;
            isBatchMode = true;
            
            // for test
            imageTile = tilemapType == TilemapType.Engine ? TempEngineImage : TempFacilityImage;
        }
        public void ExitBatchMode()
        {
            isBatchMode = false;
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
            // Set image tile
            imageTilemap.SetTile(cellPosition, imageTile);
        }

        private void SetNullTile(Vector3Int cellPosition)
        {
            previewTilemap.SetTile(cellPosition, null);
            imageTilemap.SetTile(cellPosition, null);
        }

        #endregion
    }
}
