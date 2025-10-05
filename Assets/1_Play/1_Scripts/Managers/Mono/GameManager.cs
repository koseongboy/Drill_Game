using UnityEngine;
using DrillGame.Core.Engine;
using DrillGame.Core.Managers;

namespace DrillGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Fields & Properties
        private InputSystem_Actions control;

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
        }


        private void OnEnable()
        {
            control.Enable();
        }

        private void OnDisable()
        {
            control.Disable();
        }

        #endregion
    }
}
