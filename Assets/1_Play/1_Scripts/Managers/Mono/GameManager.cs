using System;
using DrillGame._1_Play._1_Scripts.ScriptableObject;
using DrillGame.Core.Engine;
using DrillGame.Core.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace DrillGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Fields & Properties
        private InputSystem_Actions control;

        [SerializeField]
        private GridManager gridManager;

        [ReadOnly]
        [SerializeField]
        private BatchMode batchMode = BatchMode.None;

        [SerializeField]
        private TilemapType tilemapType = TilemapType.Engine;


        private int placingItemId = 0;
        [FormerlySerializedAs("dataIndex")]
        [ReadOnly]
        [SerializeField]
        [Tooltip("csv데이터 찾아오기용 index")]
        private int dataId = 111001;



        private int Counter = 0;
        #endregion

        #region Singleton & initialization
        public static GameManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("GameManager Instance already exists, destroying duplicate!");
                Destroy(gameObject);
                return;
            }

            control = new InputSystem_Actions();
            //control.Player.Jump.performed += ctx => CoreTick();
            control.Player.SlowTick.performed += ctx => SlowTick();

            control.Player.StartBatch.performed += ctx => StartBatch();
            control.Player.StopBatch.performed += ctx => StopBatch();
            control.Player.EditBatch.performed += ctx => EditBatch();

            // control.Player.BatchID_1.performed += ctx => SetBatchEntity(1);
            // control.Player.BatchID_2.performed += ctx => SetBatchEntity(2);

            control.Player.Click.performed += ctx => ClickAction();
        }
        
        // 초기 실행시에만 적용될 함수입니다. 현재는 매 실행시에 실행되도록 해뒀습니다.
        private void GameInitiate()
        {
            GameViewManager.Instance.SetViewState(GameViewManager.ViewState.All);
        }

        #endregion

        #region getters & setters
        #endregion

        #region public methods

        public void BatchEntity(int itemId)
        {
            placingItemId = itemId;
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>( itemId );
            if (!itemData)
            {
                Debug.LogWarning($"전달된 ItemID 값이 이상합니다. : {itemId}");
                return;
            }

            var type = itemData.GetItemType_Enum();
            if (type != InventoryManager.ItemType.Engine
                && type != InventoryManager.ItemType.Facility)
            {
                Debug.LogWarning("엔진 or 시설이 아닌 Item이 전달되었습니다.");
                return;
            }
            
            SetBatchEntity( itemData );
            StartBatch();
        }
        
        public void SetBatchEntity(Item_Data_ itemData)
        {
            if (itemData.GetItemType_Enum() == InventoryManager.ItemType.Engine)
            {
                tilemapType = TilemapType.Engine;
                Debug.Log("배치 모드 엔티티 선택: 엔진");
            }
            else if(itemData.GetItemType_Enum() == InventoryManager.ItemType.Facility)
            {
                tilemapType = TilemapType.Facility;
                Debug.Log("배치 모드 엔티티 선택: 시설");
            }
            dataId = itemData.EntityId;
        }

        public void StartBatch()
        {
            batchMode = BatchMode.PlaceBatch;
            if (dataId == 0)
            {
                Debug.LogWarning("배치 모드 진입 실패: 선택된 엔티티가 없습니다.");
                return;
            }
            gridManager.AfterUnitPlaced += DeleteItemAfterPlaced;
            gridManager.EnterBatchMode(tilemapType, dataId, placingItemId);
        }

        //test 테스트 dev
        public void MinerBatch()
        {
            batchMode = BatchMode.PlaceBatch;
            dataId = 101011;
            gridManager.EnterBatchMode(TilemapType.Facility, dataId, placingItemId);
        }

        public void ProcessorBatch()
        {
            batchMode = BatchMode.PlaceBatch;
            dataId = 101021;
            gridManager.EnterBatchMode(TilemapType.Facility, dataId, placingItemId);
        }
        
        public void LabBatch()
        {
            batchMode = BatchMode.PlaceBatch;
            dataId = 110001;
            gridManager.EnterBatchMode(TilemapType.Facility, dataId, placingItemId);
        }
        public void EngineBatch()
        {
            batchMode = BatchMode.PlaceBatch;
            dataId = 201001;
            gridManager.EnterBatchMode(TilemapType.Engine, dataId, placingItemId);
        }
        #endregion

        #region private methods
        private void CoreTick()
        {
            BoardManager.Instance.Tick();
        }

        private void DeleteItemAfterPlaced()
        {
            InventoryManager.Instance.RemoveItemById( placingItemId );
        }
        #endregion

        #region Unity event methods

        private void Start()
        {
            GameInitiate();
        }

        private void FixedUpdate()
        {
            if (Counter < 12)   // 주기를 늘려서 너무 빠르게 진행되지 않도록 함
            {
                Counter++;
                return;
            }
            Counter = 0;
            // 점프를 누르고 있는 동안 틱 진행
            //if (control.Player.Jump.ReadValue<float>() > 0)
            {
                CoreTick();
            }

            
        }


        private void OnEnable()
        {
            control.Enable();
        }

        private void OnDisable()
        {
            control.Disable();
        }

        private void SlowTick()
        {
            CoreTick();
        }

        // REFACTOR : 후일 click에 할당되는 게 많다면 구독 변경을 통해서 클릭 액션을 관리해야함, 현재는 하나의 함수에서 분기 처리함
        private void ClickAction()
        {
            if(batchMode == BatchMode.None) return;
            else if(batchMode == BatchMode.PlaceBatch)
                gridManager.TryPlaceBatch();
            else if(batchMode == BatchMode.EditBatch)
                gridManager.TryEditBatch();
            else if(batchMode == BatchMode.DeleteBatch)
                gridManager.TryDeleteBatch();
        }

        
        private void EditBatch()
        {
            gridManager.TryEditBatch();
        }
        private void StopBatch()
        {
            Debug.Log("진입");
            placingItemId = 0;
            dataId = 0;
            batchMode = BatchMode.None;
            gridManager.AfterUnitPlaced -= DeleteItemAfterPlaced;
            gridManager.ExitBatchMode();
        }
        #endregion
    }

    enum BatchMode
    {
        None,
        PlaceBatch,
        EditBatch,
        DeleteBatch,
    }
}
