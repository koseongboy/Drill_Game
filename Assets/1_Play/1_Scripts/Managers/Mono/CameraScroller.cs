using DrillGame._1_Play._1_Scripts.Components;
using DrillGame.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DrillGame._1_Play._1_Scripts.Managers.Mono
{
    public class CameraScroller : Monobehavior_Singleton<CameraScroller>
    {
        #region Fields & Properties
        [SerializeField]
        private float minCamOffset = 3f;
        [SerializeField]
        private float maxCamOffset = 3f;
        [SerializeField]
        private float scrollSensitivity = 240f;
        [SerializeField]
        private GridManager gridManager;

        private float scrollDelta;
        
        private Vector2Int areaStart;
        private Vector2Int areaSize; 

        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters

        #endregion

        #region public methods

        public void OnScrollCamera(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Vector2 (스크롤 휠의 x, y) 중에서 y 값만 가져와서 저장
                scrollDelta = context.ReadValue<Vector2>().y;
            }
            else
            {
                // 스크롤이 멈췄을 때
                scrollDelta = 0f;
            }
        }

        public void UpdateCameraLimit( Vector2Int areaStart, Vector2Int areaSize )
        {
            this.areaStart = areaStart;
            this.areaSize = areaSize;
        }
        #endregion

        #region private methods
        private void ChangeCamPositon()
        {
            // 저장된 스크롤 값 사용
            float scrollInput = scrollDelta;
            if (scrollInput == 0f) return;

            // 마우스 스크롤은 한번 움직일 때 큰 값을 반환하므로, Time.deltaTime과 추가 상수를 사용해 부드럽게 만들어야 해.
            float moveAmount = scrollInput * scrollSensitivity * Time.deltaTime; 
            float targetY = transform.position.y + moveAmount;
            
            // 제한 범위 계산
            float mapMinY = - minCamOffset;
            float mapMaxY = areaStart.y + areaSize.y - maxCamOffset;
            targetY = Mathf.Clamp(targetY, mapMinY, mapMaxY);
            
            // 6. 카메라 위치 업데이트
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        }
        #endregion

        #region Unity event methods
        private void Update()
        {
            ChangeCamPositon();
        }
        #endregion
    }
}