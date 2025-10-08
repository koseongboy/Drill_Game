using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

using DrillGame.View.Helper;

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
        private Transform EngineTileamp;
        [SerializeField] 
        private Transform FacilityTileamp;
        [SerializeField]    
        private Tilemap previewTilemap;
        [SerializeField] 
        private Tilemap imageTilemap;

        [SerializeField]
        private TileBase unableTile;
        [SerializeField]
        private TileBase ableTile;

        [Header("Temp : 후일 프리팹 동적 로더 제작 해주세요")]
        [SerializeField]
        private TileBase TempEngineImage;
        [SerializeField]
        private TileBase TempFacilityImage;
        [SerializeField]
        private GameObject TempEngine;
        [SerializeField]
        private GameObject TempFacility;

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
        private GameObject entityObject; 
        private Transform entityParent;

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


       

        public void TryBatch()
        {
            if (!isBatchMode)
            {
                Debug.Log("분명 게임 매니저에서 막았다고 생각했는데 다시 확인해보자");
                return;
            }


            Vector3Int cellPosition = GetCellPosition();
            
            // 배치 가능하면 실제로 배치합니다

            // Instantiate entity
            GameObject gameObject = Instantiate(entityObject, CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, entityParent);
            if(gameObject.TryGetComponent<IDrillGameObjectInit>(out var init))
            {
                init.Initialize((Vector2Int)cellPosition);
                // set sorting layer in parent
                gameObject.GetComponent<SpriteRenderer>().sortingLayerID = entityParent.GetComponent<Tilemap>().GetComponent<TilemapRenderer>().sortingLayerID;
            }
            else
            {
                Debug.LogError("IDrillGameObjectInit 인터페이스를 구현하지 않은 오브젝트입니다.");
            }
        }

        public void EnterBatchMode(TilemapType type)
        {
            tilemapType = type;
            isBatchMode = true;
            
            // for test
            if(tilemapType == TilemapType.Engine)
            {
                imageTile = TempEngineImage;
                entityObject = TempEngine;
                entityParent = EngineTileamp;
            }
            else
            {
                imageTile = TempFacilityImage;
                entityObject = TempFacility;
                entityParent = FacilityTileamp;
            }
        }
        public void ExitBatchMode()
        {
            ClearAllPreviewTile();
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
            previewTilemap.SetTile(cellPosition, ableTile);
            // Set image tile
            imageTilemap.SetTile(cellPosition, imageTile);
        }

        private void SetNullTile(Vector3Int cellPosition)
        {
            previewTilemap.SetTile(cellPosition, null);
            imageTilemap.SetTile(cellPosition, null);
        }

        private void ClearAllPreviewTile()
        {
            previewTilemap.ClearAllTiles();
            imageTilemap.ClearAllTiles();
            previousCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        }
        #endregion
    }
}
