using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace DrillGame.WindowControl
{
  public class WindowController : MonoBehaviour
    {
        // C#에서 Windows API 함수를 호출하기 위해 DllImport 사용
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        // 창 스타일을 가져오는 함수
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        // 창 스타일을 설정하는 함수
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref RECT pvParam, uint fWinIni);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // 💡 핫키 관련 상수
        // 키보드 조합 (Modifiers)
        private const uint MOD_ALT = 0x0001;     // Alt 키

        // 핫키 ID (각 핫키는 고유 ID를 가져야 함)
        private const int HOTKEY_ID = 9000;

        // 가상 키 코드 (VK Code) - 원하는 키의 코드를 사용합니다.
        private const uint VK_T = 0x54; // 'T' 키의 가상 키 코드 (예시)

        private const uint SPI_GETWORKAREA = 0x0030; // 작업 영역 가져오기 플래그

        // 창 상태 설정 상수
        const uint SWP_SHOWWINDOW = 0x0040;
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1); // 항상 위
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2); // 항상 위 아님

        public IntPtr windowHandle;
        public bool isExpanded = true;

        // 게임 창의 빌드된 실행 파일 이름 (예: MyGame.exe)
        private const string WindowTitle = "Drill Game"; // 빌드 설정의 'Product Name'과 일치해야 함

        [Header("Window Size (Percentage of Screen)")]
        [Range(0.01f, 1.0f)] // 인스펙터에서 1% ~ 100% 범위로 슬라이더 제공
        public float ExpandedWidthPercent = 0.2f;
        [Range(0.01f, 1.0f)]
        public float ExpandedHeightPercent = 1f;

        // 💡 Collapsed 크기 (예: 화면 너비의 5%, 높이의 5%)
        [Range(0.01f, 1.0f)]
        public float CollapsedWidthPercent = 0.2f;
        [Range(0.01f, 1.0f)]
        public float CollapsedHeightPercent = 0.1f;
        // 💡 후킹 관련 DllImport
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // 후킹 타입: 전역 저수준 키보드
        private const int WH_KEYBOARD_LL = 13;

        // 키보드 메시지
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104; // Alt 키와 조합된 시스템 키 다운
        private const int WM_SYSKEYUP = 0x0105; // Alt 키와 조합된 시스템 키 업

        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // 💡 메인 스레드로 데이터를 전달하기 위한 큐 (매우 중요)
        private static Queue<int> keyEventQueue = new Queue<int>();
        private static object queueLock = new object();

        // 점수 시스템을 위한 변수 (예시)
        public int totalKeyPresses = 0;

        // 창 스타일 관련 상수
        const int GWL_STYLE = -16;

        // 테두리가 있는 창 스타일
        const int WS_BORDER = 0x00800000;
        const int WS_CAPTION = 0x00C00000;
        public static readonly int WS_POPUP = unchecked((int)0x80000000); // 테두리가 없는 팝업 창 스타일

        // SetWindowPos 관련 상수
        const uint SWP_FRAMECHANGED = 0x0020;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;


        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // 1. 메시지 처리
            if (nCode >= 0)
            {
                // 키가 눌린 메시지 (KEYUP은 필요없으므로 제외)
                if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                {
                    // 키 코드를 읽음
                    int vkCode = Marshal.ReadInt32(lParam);

                    // 2. 💡 키 입력 사실을 큐에 추가 (스레드 안전성 확보)
                    lock (queueLock)
                    {
                        keyEventQueue.Enqueue(vkCode);
                    }

                    // 3. 🚨 중요: 이 시점에서 return CallNextHookEx(...)를 호출하여 
                    // 이벤트를 시스템의 다음 후크 체인으로 전달합니다.
                    // 이벤트를 가로채지 않으므로 다른 프로그램에도 키 입력이 전달됩니다.
                }

            }

            // 4. 다음 후크로 메시지를 전달 (이것이 키 입력을 투과시키는 핵심)
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }
        void Start()
        {
            // ... (창 핸들을 찾는 코드) ...
            windowHandle = FindWindow(null, WindowTitle);
            if (windowHandle == IntPtr.Zero)
            {
                Debug.LogError("Window handle not found.");
                return;
            }

            // 2. 💡 작업 영역 (Work Area) 크기 가져오기 (Taskbar 제외)
            RECT workArea = new RECT();
            // SPI_GETWORKAREA를 사용하여 작업 표시줄을 제외한 화면 영역을 가져옵니다.
            SystemParametersInfo(SPI_GETWORKAREA, 0, ref workArea, 0);

            int workAreaWidth = workArea.Right - workArea.Left;
            int workAreaHeight = workArea.Bottom - workArea.Top;

            // 3. 💡 현재 창의 크기 가져오기
            RECT windowRect;
            GetWindowRect(windowHandle, out windowRect);

            int windowWidth = windowRect.Right - windowRect.Left;
            int windowHeight = windowRect.Bottom - windowRect.Top;

            // 4. 💡 목표 위치 계산 (화면 중앙 X, 바닥에서 50px 떨어진 Y)

            // X 계산: 작업 영역 중앙 정렬
            int targetX = workArea.Left + (workAreaWidth - windowWidth) / 2;

            // Y 계산: 작업 영역 바닥(workArea.Bottom) - 창 높이 - 50px
            int targetY = workArea.Bottom - windowHeight;

            // 5. 💡 창 위치 설정 (크기는 변경하지 않음)
            SetWindowPos(windowHandle, HWND_NOTOPMOST,
                        targetX, targetY,
                        0, 0,
                        SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);

            if (windowHandle != IntPtr.Zero)
            {

                // 💡 1. 현재 창 스타일 가져오기
                int currentStyle = GetWindowLong(windowHandle, GWL_STYLE);

                // 💡 2. 타이틀 바와 경계선 스타일만 제거하여 새로운 스타일 설정
                // WS_POPUP 대신, 기존 스타일에서 캡션과 보더만 지웁니다.
                int newStyle = currentStyle & ~WS_CAPTION & ~WS_BORDER;

                SetWindowLong(windowHandle, GWL_STYLE, newStyle); // 새로운 스타일 적용

                // 💡 3. 스타일 변경 사항을 적용하고 창 위치 조정
                // 크기나 위치 변경 없이 프레임 변경을 강제합니다.
                SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, 0, 0,
                            SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);


                // 초기 위치 및 크기 설정 (기존 코드)
                SetWindowPosition(isExpanded);
            }

            // 💡 전역 키 후킹 등록
            _proc = HookCallback; // 콜백 함수 지정

            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                IntPtr hModule = GetModuleHandle(curModule.ModuleName);
                // WH_KEYBOARD_LL (13)로 저수준 키보드 후킹 등록
                hookId = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hModule, 0);

                if (hookId == IntPtr.Zero) Debug.LogError("Failed to register keyboard hook.");
            }

        }

        /// <summary>
        /// 창 위치와 크기를 설정하는 함수
        /// </summary>
        void SetWindowPosition(bool expand)
        {
            // 현재 화면 해상도를 가져옵니다.
            int screenWidth = Screen.currentResolution.width;
            int screenHeight = Screen.currentResolution.height;

            // 1. 사용할 퍼센트 값 선택
            float widthPercent = expand ? ExpandedWidthPercent : CollapsedWidthPercent;
            float heightPercent = expand ? ExpandedHeightPercent : CollapsedHeightPercent;

            // 2. 💡 픽셀 값 계산: (화면 해상도) * (퍼센트 값)
            // float으로 계산 후 int로 명시적 변환 (소수점 버림)
            int newWidth = (int)(screenWidth * widthPercent);
            int newHeight = expand ? (int)(screenHeight * heightPercent) - 90 : 60; // 확장 시 상하단 45픽셀씩 여유

            // 3. 새로운 X 좌표(창의 왼쪽 상단) 계산:
            // (화면 오른쪽 끝) - (새로운 창 너비)
            int newX = screenWidth - newWidth;

            // Y 좌표 (오른쪽 상단 고정: Y=0)
            int newY = screenHeight - 47 - newHeight;

            // 4. 창 위치, 크기 설정 (Windows API 호출)
            SetWindowPos(windowHandle, HWND_TOPMOST, newX, newY, newWidth, newHeight, SWP_SHOWWINDOW);
        }

        /// <summary>
        /// 토글 버튼에 연결될 함수
        /// </summary>
        public void ToggleWindowSize()
        {
            if (windowHandle == IntPtr.Zero) return;

            isExpanded = !isExpanded;
            SetWindowPosition(isExpanded);
        }

        // 테스트를 위해 키 입력으로 토글 기능 추가
        void Update()
        {
            // 💡 핫키가 눌렸을 때의 처리 (예: 토글 기능 호출)
            // 핫키를 등록하면, Alt+T가 눌렸을 때 유니티의 Input.GetKey(KeyCode.T)가 작동하도록 OS가 메시지를 보냅니다.
            // 하지만 이 방식은 OS 환경에 따라 안정적이지 않을 수 있습니다.

            // 🚨 더 확실한 방법: Windows 메시지 후킹이 필요하지만, 이는 C# 단독으로 복잡합니다.
            // 여기서는 간단하게 등록된 핫키가 작동했다고 가정하고 일반 입력으로 처리합니다.
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    // 토글 기능 호출
                    ToggleWindowSize();
                }
            }

            // 💡 큐에서 키 입력 이벤트 처리 (메인 스레드 안전)
            lock (queueLock)
            {
                while (keyEventQueue.Count > 0)
                {
                    int vkCode = keyEventQueue.Dequeue();

                    // 키가 눌렸다는 사실에 따라 점수를 부여
                    totalKeyPresses++;
                    Debug.Log($"Key Pressed Globally: {vkCode}. Total Score: {totalKeyPresses}");

                    // 여기에 점수를 증가시키는 Unity 로직을 구현합니다.
                    // (예: FindObjectOfType<ScoreManager>().AddScore(10);)
                }
            }

            // ... (기존 Update 함수 내의 다른 로직 유지)
        }

        void OnApplicationQuit()
        {
            // 💡 후킹 해제 (반드시 필요)
            if (hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hookId);
                Debug.Log("Global Keyboard Hook unregistered.");
            }
        }
    }
}
