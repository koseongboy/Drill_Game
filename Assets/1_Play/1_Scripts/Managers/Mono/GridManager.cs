using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Linq;

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

        private HashSet<Vector2Int> EngineOccupiedPositions = new();
        private HashSet<Vector2Int> FacilityOccupiedPositions = new();

        [ReadOnly, SerializeField]
        private Vector3 mouseWorldPosition;

        private bool isBatchMode;
        private TilemapType tilemapType;
        private TileBase imageTile;
        private GameObject entityObject; 
        private Transform entityParent;
        private List<Vector2Int> formationPositions;

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


       

        public void TryBatch()
        {
            if (!isBatchMode)
            {
                Debug.Log("분명 게임 매니저에서 막았다고 생각했는데 다시 확인해보자");
                return;
            }


            Vector3Int cellPosition = GetCellPosition();
            Vector2Int cellPos2D = (Vector2Int)cellPosition;

            // 배치 가능하면 실제로 배치합니다
            
            HashSet<Vector2Int> occupiedPositions = tilemapType == TilemapType.Engine ? EngineOccupiedPositions : FacilityOccupiedPositions;
            Vector3Int[] ablePositions;

            if (!CanPlaceTile(occupiedPositions, cellPos2D, formationPositions, out ablePositions, out _))
            {
                Debug.Log("해당 위치에 배치할 수 없습니다.");
                return;
            }

            // Instantiate entity
            GameObject gameObject = Instantiate(entityObject, grid.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, entityParent);
            if(gameObject.TryGetComponent<IDrillGameObjectInit>(out var init))
            {
                init.Initialize(cellPos2D);
                // set sorting layer in parent
                gameObject.GetComponent<SpriteRenderer>().sortingLayerID = entityParent.GetComponent<Tilemap>().GetComponent<TilemapRenderer>().sortingLayerID;
                
                if(tilemapType == TilemapType.Engine)
                {
                    EngineOccupiedPositions.Add(cellPos2D);
                }
                else
                {
                    FacilityOccupiedPositions.Add(cellPos2D);
                }
            }
            else
            {
                Debug.LogError("IDrillGameObjectInit 인터페이스를 구현하지 않은 오브젝트입니다.");
            }

            // hashset에 점유 공간 추가
            occupiedPositions.UnionWith(ablePositions.Select(v => (Vector2Int)v));
        }

        public void EnterBatchMode(TilemapType type)
        {
            tilemapType = type;
            isBatchMode = true;

            // 진입 직후 이전 위치에 타일이 남아있을 수 있으므로 제거
            if(previousCellPosition.x != int.MinValue)
                SetNullTile(previousCellPosition);

            // for test 후에 동적 로드 필요 todo 

            if (tilemapType == TilemapType.Engine)
            {
                imageTile = TempEngineImage;
                entityObject = TempEngine;
                entityParent = EngineTileamp;

                formationPositions = new() {
                    new Vector2Int(0, 0),
                    new Vector2Int(-1, 0),
                    new Vector2Int(1, 0)
                };
            }
            else
            {
                imageTile = TempFacilityImage;
                entityObject = TempFacility;
                entityParent = FacilityTileamp;

                formationPositions = new() {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(1, 1)
                };
            }



            // 진입 직후    미리보기 설정을 위한 previousCellPosition 초기화
            previousCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        }
        public void ExitBatchMode()
        {
            //ClearAllPreviewTile();
            if(previousCellPosition.x != int.MinValue)
                SetNullTile(previousCellPosition);
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

        

        private bool CanPlaceTile(HashSet<Vector2Int> occupiedPositions, Vector2Int centerPosition, List<Vector2Int> formationPositions, out Vector3Int[] ablePositions, out Vector3Int[] disablePositions)
        {
            List<Vector3Int> ablePosList = new();
            List<Vector3Int> disablePosList = new();
            foreach (var formation in formationPositions)
            {
                Vector2Int checkPos = centerPosition + formation;
                if (occupiedPositions.Contains(checkPos))
                {
                    // 겹치는 위치가 있으면 배치 불가
                    disablePosList.Add((Vector3Int)checkPos);
                }
                else
                {
                    ablePosList.Add((Vector3Int)checkPos);
                }
            }
            ablePositions = ablePosList.ToArray();
            disablePositions = disablePosList.ToArray();
            
            return disablePosList.Count == 0;
        }

        private Vector3Int GetCellPosition()
        {
            mouseWorldPosition = Mouse.current.position.ReadValue();
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, distanceToCamera)
            );
            Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);

            return cellPosition;
        }
        private void SetPreviewTile(Vector3 mousePosition)
        {
            Vector3Int cellPosition = grid.WorldToCell(mousePosition);
            
            // Set image tile
            imageTilemap.SetTile(cellPosition, imageTile);

            // Set preview tile
            Vector3Int[] ablePositions;
            Vector3Int[] disablePositions;
            TileBase[] ableTiles;
            TileBase[] disableTiles;

            CanPlaceTile(
                tilemapType == TilemapType.Engine ? EngineOccupiedPositions : FacilityOccupiedPositions,
                (Vector2Int)cellPosition,
                formationPositions,
                out ablePositions,
                out disablePositions
            );

            ableTiles = Enumerable.Repeat(ableTile, ablePositions.Length).ToArray();
            disableTiles = Enumerable.Repeat(unableTile, disablePositions.Length).ToArray();


            previewTilemap.SetTiles(ablePositions, ableTiles);
            previewTilemap.SetTiles(disablePositions, disableTiles);

        }

        private void SetNullTile(Vector3Int cellPosition)
        {
            imageTilemap.SetTile(cellPosition, null);

            Vector3Int[] ablePositions;
            Vector3Int[] disablePositions;
            Vector3Int[] allPositions;
            TileBase[] nullTiles;
            CanPlaceTile(
                tilemapType == TilemapType.Engine ? EngineOccupiedPositions : FacilityOccupiedPositions,
                (Vector2Int)cellPosition,
                formationPositions,
                out ablePositions,
                out disablePositions
            );
            
            allPositions = new Vector3Int[ablePositions.Length + disablePositions.Length];
            ablePositions.CopyTo(allPositions, 0);
            disablePositions.CopyTo(allPositions, ablePositions.Length);

            nullTiles = new TileBase[allPositions.Length];

            previewTilemap.SetTiles(allPositions, nullTiles);
        }

        private void ClearAllPreviewTile()
        {
            // todo? 이거 좀 비효율적인거 같은데 -> 예외 처리가 더 많다 그냥 쓰자 -> 으 될지도
            previewTilemap.ClearAllTiles();
            imageTilemap.ClearAllTiles();
            previousCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
        }
        #endregion
    }
}
