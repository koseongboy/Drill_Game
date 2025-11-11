using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrillGame.WindowControl
{
    public class WindowDragHandler : MonoBehaviour, IPointerDownHandler
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();



        // WindowController에서 창 핸들을 가져오기 위함
        private WindowController windowController;

        // Windows 메시지 상수
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2; // 창의 타이틀 바 영역




        void Start()
        {
            // WindowController 스크립트 찾기 (창 핸들을 가져오기 위해)
            windowController = FindFirstObjectByType<WindowController>();

            if (windowController == null)
            {
                Debug.LogError("WindowController 스크립트를 찾을 수 없습니다. 창 핸들 접근 불가.");
            }
        }

        // 참고: OnPointerDown 함수는 dragStartPos = Input.mousePosition;만 수행하도록 유지합니다.

        // 💡 마우스 버튼을 누르는 순간 호출되는 유니티 인터페이스 함수
        public void OnPointerDown(PointerEventData eventData)
        {
            if (windowController == null || windowController.windowHandle == IntPtr.Zero)
            {
                return;
            }

            // 1. 마우스 캡처 해제
            ReleaseCapture();

            // 2. 창에 타이틀 바 클릭 메시지 전송
            // 0xA1 (WM_NCLBUTTONDOWN), 0x2 (HT_CAPTION)
            // 파라미터는 상수 변수 이름 대신 16진수 값을 직접 사용합니다.
            SendMessage(windowController.windowHandle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            // 또는: SendMessage(windowController.windowHandle, 0xA1, 0x2, 0);
        }


    }
}
