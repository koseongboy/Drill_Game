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
            // 물리 연산 주기마다 코어 틱을 진행
        }
        private void Awake()
        {
            control = new InputSystem_Actions();
            control.Player.Jump.performed += ctx => CoreTick();
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
