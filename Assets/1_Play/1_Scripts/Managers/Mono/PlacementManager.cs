using UnityEngine;
using UnityEngine.InputSystem;

namespace DrillGame.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        #region Fields & Properties
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
        #endregion

        #region Unity event methods
        private void Awake()
        {
            distanceToCamera = -Camera.main.transform.position.z;
        }
        private void Update()
        {
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            mouseWorldPosition = Mouse.current.position.ReadValue();
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, distanceToCamera)
            );
            

        }
        #endregion
    }
}
