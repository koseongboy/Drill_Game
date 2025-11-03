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
        private bool batchMode = false;

        [ReadOnly]
        [SerializeField]
        [Tooltip ("배치 모드에서 선택된 엔티티의 ID 값 -1 : 선택 안됨")]
        private int idValue = -1;

        [SerializeField]
        private TilemapType tilemapType = TilemapType.Engine;




        private int Counter = 0;
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
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
        private void Awake()
        {
            control = new InputSystem_Actions();
            //control.Player.Jump.performed += ctx => CoreTick();
            control.Player.SlowTick.performed += ctx => SlowTick();

            control.Player.StartBatch.performed += ctx => StartBatch();
            control.Player.StopBatch.performed += ctx => StopBatch();
            control.Player.EditBatch.performed += ctx => EditBatch();

            control.Player.BatchMode_1.performed += ctx => SetBatchEntity(1);
            control.Player.BatchMode_2.performed += ctx => SetBatchEntity(2);

            control.Player.Attack.performed += ctx => TryBatch();
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

        private void TryBatch()
        {
            if (!batchMode) return;
            
            gridManager.TryBatch();
        }

        private void StartBatch()
        {
            batchMode = true;
            if(idValue == -1)
            {
                Debug.LogWarning("배치 모드 진입 실패: 선택된 엔티티가 없습니다.");
                return;
            }
            
            gridManager.EnterBatchMode(tilemapType, idValue);
        }
        private void EditBatch()
        {
            if (!batchMode) return;

            gridManager.EditBatch();
        }
        private void StopBatch()
        { 
            idValue = -1;
            batchMode = false;
            gridManager.ExitBatchMode();
        }

        private void SetBatchEntity(int idValue)
        {
            this.idValue = idValue;

            // test
            // todo 알맞은 value 처리 필요
            if(idValue == 1)
            {
                this.tilemapType = TilemapType.Engine;
                Debug.Log("배치 모드 엔티티 선택: 엔진");
            }
            else if(idValue == 2)
            {
                this.tilemapType = TilemapType.Facility;
                Debug.Log("배치 모드 엔티티 선택: 시설");
            }
        }

        #endregion
    }
}
