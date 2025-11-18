using DrillGame.Core.Engine;
using DrillGame.Core.Managers;
using UnityEngine;

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

        [ReadOnly]
        [SerializeField]
        [Tooltip ("배치 모드에서 선택된 엔티티의 ID 값 -1 : 선택 안됨")]
        private int idValue = -1;

        [SerializeField]
        private TilemapType tilemapType = TilemapType.Engine;




        private int Counter = 0;
        #endregion

        #region Singleton & initialization
        public static GameManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                GameInitiate();
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

            control.Player.BatchID_1.performed += ctx => SetBatchEntity(1);
            control.Player.BatchID_2.performed += ctx => SetBatchEntity(2);

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
        public void SetBatchEntity(int idValue)
        {
            this.idValue = idValue;

            // test
            // todo 알맞은 value 처리 필요
            if (idValue == 1)
            {
                this.tilemapType = TilemapType.Engine;
                Debug.Log("배치 모드 엔티티 선택: 엔진");
            }
            else if (idValue == 2)
            {
                this.tilemapType = TilemapType.Facility;
                Debug.Log("배치 모드 엔티티 선택: 시설");
            }
        }

        public void StartBatch()
        {
            batchMode = BatchMode.PlaceBatch;
            if (idValue == -1)
            {
                Debug.LogWarning("배치 모드 진입 실패: 선택된 엔티티가 없습니다.");
                return;
            }

            gridManager.EnterBatchMode(tilemapType, idValue);
        }
        #endregion

        #region private methods
        private void CoreTick()
        {
            BoardManager.Instance.Tick();
        }
        #endregion

        #region Unity event methods
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
            idValue = -1;
            batchMode = BatchMode.None;
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
