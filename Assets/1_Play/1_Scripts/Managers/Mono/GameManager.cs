using UnityEngine;
using DrillGame.Core.Engine;
using DrillGame.Core.Managers;

namespace DrillGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Fields & Properties
        private InputSystem_Actions control;

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
            if (Counter < 10)   // 주기를 늘려서 너무 빠르게 진행되지 않도록 함
            {
                Counter++;
                return;
            }
            Counter = 0;
            // 점프를 누르고 있는 동안 틱 진행
            if (control.Player.Jump.ReadValue<float>() > 0)
            {
                CoreTick();
            }
        }
        private void Awake()
        {
            control = new InputSystem_Actions();
            //control.Player.Jump.performed += ctx => CoreTick();
            control.Player.SlowTick.performed += ctx => SlowTick();
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

        #endregion
    }
}
