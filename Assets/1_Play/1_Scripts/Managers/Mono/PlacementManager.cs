using UnityEngine;
using UnityEngine.InputSystem;

namespace DrillGame.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        #region Fields & Properties
        private InputSystem_Actions controls;

        [SerializeField]
        private GridManager gridManager;

        [ReadOnly]
        [SerializeField]
        private Vector3 mouseWorldPosition;

        private float distanceToCamera;

        
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        #endregion

        #region private methods
        private void OnClicked(InputAction.CallbackContext context)
        {
            Debug.Log($"Clicked at World Position: {mouseWorldPosition}");
            gridManager.SetPreviewTile(mouseWorldPosition);

        }
        #endregion

        #region Unity event methods
        private void Awake()
        {
            controls = new InputSystem_Actions();
            controls.Player.Attack.performed += OnClicked;
            distanceToCamera = -Camera.main.transform.position.z;
        }
        private void OnEnable()
        {
            controls.Player.Enable();
        }
        private void OnDisable()
        {
            controls.Player.Disable();
        }
        private void Update()
        {
            // 마우스 위치를 월드 좌표로 변환
            mouseWorldPosition = Mouse.current.position.ReadValue();
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, distanceToCamera)
            );
            
            

        }

        
        #endregion
    }
}
