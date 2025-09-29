using UnityEngine;
using UnityEngine.InputSystem;

using DrillGame.UI;

namespace DrillGame
{
    public class DebugInputTestManager : MonoBehaviour
    {
        // 1. Input Actions 에셋에서 생성된 C# 클래스 인스턴스
        private InputSystem_Actions controls;

        // 이 클래스 이름은 Input Actions 파일명에 따라 다를 수 있습니다.
        // 만약 파일명이 "PlayerControls"라면, "PlayerControls controls;"로 바꿔야 합니다.

        private void Awake()
        {
            // 인스턴스 생성
            controls = new InputSystem_Actions();

            // 2. Action Map(Player)의 특정 Action(Jump)에 이벤트 함수 연결 (구독)
            // 'performed'는 키를 누르는 순간(Down)에 해당합니다.
            controls.Player.Jump.performed += OnJumpPerformed;

            // 만약 키를 뗄 때(Up) 작동하게 하려면 'canceled' 이벤트를 사용하면 됩니다.
            // controls.Player.Jump.canceled += OnJumpCanceled;
        }

        private void OnEnable()
        {
            // 3. Action Map 활성화 (필수)
            controls.Player.Enable();
        }

        private void OnDisable()
        {
            // 오브젝트가 비활성화될 때 비활성화 (필수)
            controls.Player.Disable();
        }

        // 4. 이벤트가 발생했을 때 호출될 함수 (콜백 함수)
        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("Jump Action (Spacebar) Performed! - Load UI");
            UILoader.Instance.LoadUI("UI_Facility_Core");
        }
    }
}
