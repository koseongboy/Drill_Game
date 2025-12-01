using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Linq;
using DrillGame._1_Play._1_Scripts.Managers.Mono;
using DrillGame.View.Helper;
using UnityEngine.Serialization;
using System;
using DrillGame.Core.Managers;

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
        [FormerlySerializedAs("BackGroundTilemap")] [FormerlySerializedAs("BackGroundTileamp")] [SerializeField]
        private Transform FactoryBackGroundTilemap;        
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

        [FormerlySerializedAs("GridBGImage")]
        [Header("Temp : 후일 프리팹 동적 로더 제작 해주세요")]
        [SerializeField]
        private TileBase FactoryGridImage;
        [SerializeField]
        private TileBase FactoryBoundaryImage;
        [SerializeField]
        private TileBase TempEngineImage;
        [SerializeField]
        private TileBase TempFacilityImage;
        [SerializeField]
        private GameObject TempEngine;
        [SerializeField]
        private GameObject TempFacility;

        private Action updateAction;
        private float distanceToCamera;
        private Grid grid;

        private Vector3Int previousCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        [FormerlySerializedAs("InitialAreaStart")]
        [Header("공장 구역 Grid의 크기")]
        [SerializeField]
        private Vector2Int AreaStart = Vector2Int.zero; // 구역의 좌측 하단 좌표 (예: (0, 0))
        [SerializeField]
        private Vector2Int AreaSize = new Vector2Int(10, 10); // 구역의 가로/세로 크기 (예: 10x10)
        
        
        private HashSet<Vector2Int> placeableAreaPositions = new();
        private HashSet<Vector2Int> EngineOccupiedPositions = new();
        private HashSet<Vector2Int> FacilityOccupiedPositions = new();

        [ReadOnly, SerializeField]
        private Vector3 mouseWorldPosition;
        private BatchMode batchMode;
        private TilemapType tilemapType;
        private TileBase imageTile;
        private GameObject entityObject; 
        [SerializeField] private GameObject facilityObject;
        private Transform entityParent;
        private List<Vector2Int> formationPositions;

        private int placingItemId;
        private int placingEntityId;
        
        public event Action AfterUnitPlaced;

        int level = 1; //todo 레벨 저장 구현 필요

        #endregion

        #region Singleton & initialization
        private void Initialize()
        {
            
            distanceToCamera = -Camera.main.transform.position.z;
            grid = GetComponent<Grid>();
            InitPlaceableArea();

            // 엔티티 삭제관련 구독처리
            BoardManager.Instance.OnEntityDeleted += ClearOccupiedPositions;

            // 코어 배치
            CoreBatch();
            DrillBatch();

        }

        private void InitPlaceableArea()
        {
            // TODO: 공장의 레벨에 따라, 엑셀로부터 Height를 가지고 오는 로직 필요.
            
            // 이미 PlaceableAreaPositions가 있다면 초기화
            placeableAreaPositions.Clear(); 
            Vector2Int start = AreaStart;
            Vector2Int end = start + AreaSize;
            
            List<Vector2Int> newAddTilePosisions = new List<Vector2Int>();
            
            // 모든 타일 좌표를 반복하며 HashSet에 추가
            for (int x = start.x; x < end.x; x++)
            {
                for (int y = start.y; y < end.y; y++)
                {
                    Vector2Int pos2D = new Vector2Int(x, y);
                    placeableAreaPositions.Add(new Vector2Int(x, y));
                    newAddTilePosisions.Add(pos2D);
                }
            }
            UpdateTileMap_Factory(newAddTilePosisions); // 타일맵 Sprite 새로 배치해주기
            
            // Debug.Log($"PlaceableAreaPositions에 {placeableAreaPositions.Count}개의 타일이 추가되었습니다.");
        }

        #endregion

        #region getters & setters

        public Vector2Int GetAreaStart()
        {
            return AreaStart;
        }

        public Vector2Int GetAreaSize()
        {
            return AreaSize;
        }
        
        #endregion

        #region public methods


        public void TryEditBatch()
        {
            SwitchUpdateAction(batchMode);
            Debug.Log("배치 수정");
            // for test
            TryDeleteBatch();
        }

        public void TryDeleteBatch()
        {
            SwitchUpdateAction(batchMode);

        }

        public void TryPlaceBatch()
        {
            // update Action 전환
            SwitchUpdateAction(batchMode);

            Vector3Int cellPosition = GetCellPosition();
            Vector2Int cellPos2D = (Vector2Int)cellPosition;

            // 배치 가능하면 실제로 배치합니다
            
            HashSet<Vector2Int> occupiedPositions = tilemapType == TilemapType.Engine ? EngineOccupiedPositions : FacilityOccupiedPositions;
            Vector3Int[] ablePositions;

            if (!CanPlaceTile(occupiedPositions, cellPos2D, formationPositions, out ablePositions, out _))
            {
                Debug.Log("해당 위치에 배치할 수 없습니다");
                return;
            }

            // Instantiate entity
            GameObject gameObject = Instantiate(entityObject, grid.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, entityParent);
            if(gameObject.TryGetComponent<IDrillGameObjectInit>(out var init))
            {
                init.Initialize(cellPos2D, placingItemId, placingEntityId);
                // set sorting layer in parent
                gameObject.GetComponent<SpriteRenderer>().sortingLayerID = entityParent.GetComponent<Tilemap>().GetComponent<TilemapRenderer>().sortingLayerID;
        
            }
            else
            {
                Debug.LogError("IDrillGameObjectInit 인터페이스를 구현하지 않은 오브젝트입니다.");
            }

            // hashset에 점유 공간 추가
            occupiedPositions.UnionWith(ablePositions.Select(v => (Vector2Int)v));
            
            // 인벤토리에 있는 Item을 없애주기 위한 Action호출
            AfterUnitPlaced?.Invoke();


            // 배치 모드 종료
            GameManager.Instance.StopBatch();
        }

        public void DrillBatch()
        {
            // update Action 전환

            Vector3Int cellPosition = new Vector3Int(-1, -6, 0);
            Vector2Int cellPos2D = (Vector2Int)cellPosition;
            HashSet<Vector2Int> occupiedPositions = FacilityOccupiedPositions;
            occupiedPositions.Add(cellPos2D);

            EnterBatchMode(TilemapType.Facility, 112000 + level);
            
            // Instantiate entity
            GameObject gameObject = Instantiate(entityObject, grid.CellToWorld(cellPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, entityParent);
            if(gameObject.TryGetComponent<IDrillGameObjectInit>(out var init))
            {
                init.Initialize(cellPos2D, 0, 112000 + level); // TODO : 레벨에 맞는 드릴 설치해줘야.
                // set sorting layer in parent
                gameObject.GetComponent<SpriteRenderer>().sortingLayerID = entityParent.GetComponent<Tilemap>().GetComponent<TilemapRenderer>().sortingLayerID;
            }
            else
            {
                Debug.LogError("IDrillGameObjectInit 인터페이스를 구현하지 않은 오브젝트입니다.");
            }
            
            ExitBatchMode();
        }


        public void EnterBatchMode(TilemapType type, int entityId, int itemId = 0)
        {
            tilemapType = type;
            placingItemId = itemId;
            placingEntityId = entityId;

            
            // 진입 직후 이전 위치에 타일이 남아있을 수 있으므로 제거
            if (previousCellPosition.x != int.MinValue)
                SetNullTile(previousCellPosition);

            // for test 후에 동적 로드 필요 todo , id 처리도 필요

            if (tilemapType == TilemapType.Engine)
            {
                imageTile = TempEngineImage;
                entityObject = TempEngine;
                entityParent = EngineTileamp;
                List<Vector2Int> newCoordinates = new();
                foreach (var coord in ScriptableObjectManager.Instance.GetData<Engine_Data_>(entityId).GetCoordinates())
                {
                    newCoordinates.Add(coord - ScriptableObjectManager.Instance.GetData<Engine_Data_>(entityId).GetMainCoordinate());
                }
                formationPositions = newCoordinates;
            }
            else
            {
                imageTile = TempFacilityImage;
                entityObject = TempFacility;
                entityParent = FacilityTileamp;
                formationPositions = ScriptableObjectManager.Instance.GetData<Facility_Data_>(entityId).GetCoordinates();
            }

            // 진입 직후    미리보기 설정을 위한 previousCellPosition 초기화
            previousCellPosition = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

            batchMode = BatchMode.PlaceBatch;
            SwitchUpdateAction(batchMode);
        }

        
        public void ExitBatchMode()
        {
            //ClearAllPreviewTile();
            if(previousCellPosition.x != int.MinValue)
                SetNullTile(previousCellPosition);

            batchMode = BatchMode.None;
            SwitchUpdateAction(batchMode);
        }


        /// <summary>
        /// 공장을 확장하고 설치 가능한 좌표 목록을 업데이트합니다. 공장 레벨업 시 호출.
        /// </summary>
        /// <param name="newAreaPositions">새로 설치 가능 구역이 되는 타일 좌표 목록.</param>
        public void ExpandPlaceableArea(int newHeight) {
            // 파라미터 유효성 검사
            if (newHeight <= AreaSize.y)
            {
                Debug.LogWarning($"새로운 높이({newHeight})가 현재 높이({AreaSize.y})보다 크지 않습니다. 확장하지 않습니다.");
                return;
            }

            // 확장할 구역의 경계 계산
            Vector2Int startPoint = AreaStart;
            int currentWidth = AreaSize.x;
            int startY = AreaSize.y; // 현재 높이부터 시작
            int endY = newHeight; // 최종 도달할 높이
            
            HashSet<Vector2Int> newAddedTiles = new HashSet<Vector2Int>();
            int newTileCount = 0;

            // x는 InitialAreaStart.x부터 currentWidth까지 (폭 고정)
            for (int x = startPoint.x; x < startPoint.x + currentWidth; x++)
            {
                // y는 현재 높이부터 새로운 높이까지
                for (int y = startPoint.y + startY; y < startPoint.y + endY; y++)
                {
                    Vector2Int newPos = new Vector2Int(x, y);
                    
                    if (placeableAreaPositions.Add(newPos))
                    {
                        newAddedTiles.Add(newPos);
                        newTileCount++;
                    }
                }
            }
            
            AreaSize.y = newHeight;
            CameraScroller.Instance.UpdateCameraLimit(AreaStart, AreaSize);
            
            // 타일맵 sprite 다시 배치
            UpdateTileMap_Factory(newAddedTiles);
        }
        #endregion

        #region private methods
        private void CoreBatch()
        {
            //return;
            // 실제 배치는 이루어지지 않고, 타일 점유만 해준다. 배치는 씬에서 이루어짐
            List<Vector2Int> coreFormation = CoreManager.Instance.GetCoreFormations();
            EngineOccupiedPositions.UnionWith(coreFormation);
            FacilityOccupiedPositions.UnionWith(coreFormation);

        }


        private void SwitchUpdateAction(BatchMode mode)
        {
            switch (mode)
            {
                case BatchMode.None:
                    updateAction = null;
                    break;
                case BatchMode.PlaceBatch:
                    updateAction = Update_Place;
                    break;
                case BatchMode.EditBatch:
                    updateAction = Update_Delete;
                    break;
                case BatchMode.DeleteBatch:
                    updateAction = Update_Delete;
                    break;
                default:
                    updateAction = null;
                    break;
            }
        }

        private void ClearOccupiedPositions(List<Vector2Int> positions, TilemapType tilemapType)
        {
            HashSet<Vector2Int> occupiedPositions = tilemapType == TilemapType.Engine ? EngineOccupiedPositions : FacilityOccupiedPositions;
            foreach (var pos in positions)
            {
                if (occupiedPositions.Contains(pos))
                {
                    occupiedPositions.Remove(pos);
                }
            }
        }

        #endregion

        #region Unity event methods
        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            CameraScroller.Instance.UpdateCameraLimit(AreaStart, AreaSize);
        }

        private void Update()
        {
            updateAction?.Invoke();

        }

        private void Update_Place()
        {
            // 배치 모드일 때의 미리보기입니다
            Vector3Int cellPosition = GetCellPosition();

            if (cellPosition != previousCellPosition)
            {
                SetNullTile(previousCellPosition);
                SetPreviewTile(mouseWorldPosition);


                previousCellPosition = cellPosition;
            }

        }

        // edit mode 에서도 동시에 사용합니다. edit 모드는 삭제후 집어 들기 판정입니다.

        // 마우스 커서 위에 있는 타일의 밝기를 조절합니다.
        private void Update_Delete()
        {
            Vector3Int cellPosition = GetCellPosition();
            // cellPosition 기반으로 타일을 get합니다

            Debug.Log(cellPosition);

        }



        private bool CanPlaceTile(HashSet<Vector2Int> occupiedPositions, Vector2Int centerPosition, List<Vector2Int> formationPositions, out Vector3Int[] ablePositions, out Vector3Int[] disablePositions)
        {
            List<Vector3Int> ablePosList = new();
            List<Vector3Int> disablePosList = new();
            bool isAllPlaceable = true;
            
            foreach (var formation in formationPositions)
            {
                Vector2Int checkPos = centerPosition + formation;
                // 1. 해당 위치가 '공장 구역' 목록에 포함되어 있는지 확인
                if (!placeableAreaPositions.Contains(checkPos)) 
                {
                    // 설치 가능 구역 밖
                    disablePosList.Add((Vector3Int)checkPos); 
                    isAllPlaceable = false; // 하나라도 구역 밖이면 최종 배치 불가능
                    continue; // 다음 위치 확인
                }

                // 2. 해당 위치가 '다른 오브젝트에 의해 이미 점유'되어 있는지 확인
                if (occupiedPositions.Contains(checkPos))
                {
                    // 이미 다른 시설이 점유한 위치
                    disablePosList.Add((Vector3Int)checkPos);
                    isAllPlaceable = false; // 하나라도 점유되어 있으면 최종 배치 불가능
                    // continue; -> 이미 위의 if문에서 포함 여부를 체크했으므로 여기서는 continue 대신 플래그만 변경해도 됨.
                }
                else
                {
                    // 설치 가능 구역 내부에 있고, 점유되지도 않은 위치
                    ablePosList.Add((Vector3Int)checkPos);
                }
            }
            ablePositions = ablePosList.ToArray();
            disablePositions = disablePosList.ToArray();

            return isAllPlaceable;
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

        private void UpdateTileMap_Factory(IEnumerable<Vector2Int> positions)
        {
            UpdateTileMap_InFactoryGrid(positions);
            UpdateTileMap_FactoryBoundary();
        }
        
        // <summary>
        /// 지정된 좌표 목록에 Grid BG Image 타일을 그립니다.
        /// </summary>
        /// <param name="positions">새로 타일을 그릴 Vector2Int 좌표 목록.</param>
        private void UpdateTileMap_InFactoryGrid(IEnumerable<Vector2Int> positions)
        {
            if (!positions.Any()) return; // 그릴 타일이 없으면 종료

            // Tilemap 컴포넌트 가져오기
            if (FactoryBackGroundTilemap.TryGetComponent<Tilemap>(out var tilemap))
            {
                Vector3Int[] positions3D = positions.Select(v => new Vector3Int(v.x, v.y, 0)).ToArray();
                TileBase[] tiles = Enumerable.Repeat(FactoryGridImage, positions3D.Length).ToArray();
                tilemap.SetTiles(positions3D, tiles);
            }
            else
            {
                Debug.LogError("Grid BG Image에 Tilemap이 없습니다.");
            }
        }
        
        /// <summary>
        /// 공장의 테두리를 그립니다.
        /// </summary>
        private void UpdateTileMap_FactoryBoundary()
        {
            // 크기가 0이거나 유효하지 않으면 그리지 않음
            if (AreaSize.x <= 0 || AreaSize.y <= 0) return;

            // 경계 좌표 계산
            Vector2Int startPoint = AreaStart;
            int currentWidth = AreaSize.x;
            int currentHeight = AreaSize.y;
    
            // 테두리 타일 좌표를 저장할 리스트
            List<Vector3Int> boundaryPositions = new List<Vector3Int>();

            // 테두리 좌표 계산
            int minX = startPoint.x - 1;
            int maxX = startPoint.x + currentWidth;
            int minY = startPoint.y - 1;
            int maxY = startPoint.y + currentHeight;

            // 상/하단 테두리
            for (int x = minX; x <= maxX; x++)
            {
                boundaryPositions.Add(new Vector3Int(x, minY, 0)); // 최하단
                boundaryPositions.Add(new Vector3Int(x, maxY, 0)); // 최상단
            }
            // 좌/우측 테두리
            for (int y = minY + 1; y < maxY; y++)
            {
                boundaryPositions.Add(new Vector3Int(minX, y, 0)); // 좌측
                boundaryPositions.Add(new Vector3Int(maxX, y, 0)); // 우측
            }

            // 3. Tilemap 컴포넌트 참조 확인
            if (!FactoryBackGroundTilemap.TryGetComponent<Tilemap>(out var tilemap))
            {
                return;
            }

            // 일괄 작업을 위한 배열 준비 (중복 제거된 최종 좌표)
            Vector3Int[] positions3D = boundaryPositions.Distinct().ToArray();
            
            // 이전 테두리 지우기
            TileBase[] nullTiles = new TileBase[positions3D.Length]; 
            tilemap.SetTiles(positions3D, nullTiles);
            
            // 새로운 테두리 그리기
            TileBase[] boundaryTiles = Enumerable.Repeat(FactoryBoundaryImage, positions3D.Length).ToArray();
            tilemap.SetTiles(positions3D, boundaryTiles);
        }
        #endregion

        #region Test
        
        [ContextMenu("Test: 공장 Grid 크기 -> 20으로 확장")]
        public void TestExpandArea()
        {
            
            ExpandPlaceableArea(20);
        }

        #endregion
    }
}
