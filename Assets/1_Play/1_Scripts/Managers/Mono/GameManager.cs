using DrillGame.Core.Engine;
using DrillGame.Core.Managers;
using System.Diagnostics;
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

            control.Player.EngineBatch.performed += ctx => EngineBatch();
            control.Player.FacilityBatch.performed += ctx => FacilityBatch();
            control.Player.StopBatch.performed += ctx => StopBatch();

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
        private void StopBatch()
        { 
            batchMode = false;
            gridManager.ExitBatchMode();
        }
        private void FacilityBatch()
        {
            batchMode = true;
            gridManager.EnterBatchMode(TilemapType.Facility);
        }

        private void EngineBatch()
        {
            batchMode = true;
            gridManager.EnterBatchMode(TilemapType.Engine);
        }

        #endregion
    }
}
