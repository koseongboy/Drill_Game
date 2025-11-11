using UnityEngine;
using System;
using System.Collections.Generic;

// Windows 플랫폼에서만 DllImport가 컴파일되도록 플랫폼 지시문을 사용합니다.
#if UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;
#endif

namespace DrillGame.WindowControl
{
  public class WindowController : MonoBehaviour
    {
        
// ------------------------------------------------------------------------------------------
// Windows 빌드 환경에서만 컴파일
// ------------------------------------------------------------------------------------------
#if UNITY_STANDALONE_WIN
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
        private const uint MOD_ALT = 0x0001;     // Alt 키
        private const int HOTKEY_ID = 9000;
        private const uint VK_T = 0x54; // 'T' 키의 가상 키 코드 (예시)
        private const uint SPI_GETWORKAREA = 0x0030; // 작업 영역 가져오기 플래그

        // 창 상태 설정 상수
        const uint SWP_SHOWWINDOW = 0x0040;
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1); // 항상 위
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2); // 항상 위 아님

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
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105; // Alt 키와 조합된 시스템 키 업

        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // 창 스타일 관련 상수
        const int GWL_STYLE = -16;
        const int WS_BORDER = 0x00800000;
        const int WS_CAPTION = 0x00C00000;
        public static readonly int WS_POPUP = unchecked((int)0x80000000); // 테두리가 없는 팝업 창 스타일

        // SetWindowPos 관련 상수
        const uint SWP_FRAMECHANGED = 0x0020;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;

        // Window Handle은 Windows에서만 유효합니다.
        public IntPtr windowHandle;

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    lock (queueLock)
                    {
                        keyEventQueue.Enqueue(vkCode);
                    }
                }
            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Windows API를 사용하여 창 위치와 크기를 설정하는 내부 함수 (Windows Only)
        /// </summary>
        void SetWindowPositionInternal(bool expand)
        {
            if (windowHandle == IntPtr.Zero) return;

            // 현재 화면 해상도를 가져옵니다. (빌드 환경에서는 Screen.currentResolution.width 사용 가능)
            int screenWidth = Screen.currentResolution.width;
            int screenHeight = Screen.currentResolution.height;

            float widthPercent = expand ? ExpandedWidthPercent : CollapsedWidthPercent;
            float heightPercent = expand ? ExpandedHeightPercent : CollapsedHeightPercent;

            int newWidth = (int)(screenWidth * widthPercent);
            // 확장 시 상하단 45픽셀씩 여유를 두거나 60px 크기로 설정
            int newHeight = expand ? (int)(screenHeight * heightPercent) - 90 : 60; 

            // X 계산: (화면 오른쪽 끝) - (새로운 창 너비)
            int newX = screenWidth - newWidth;

            // Y 좌표 (오른쪽 상단 고정: Y=0)
            int newY = screenHeight - 47 - newHeight;

            SetWindowPos(windowHandle, HWND_TOPMOST, newX, newY, newWidth, newHeight, SWP_SHOWWINDOW);
        }

        /// <summary>
        /// 창을 테두리가 없는 팝업 스타일로 설정하고 후킹을 등록하는 내부 함수 (Windows Only)
        /// </summary>
        void InitializeWindowAndHook()
        {
            windowHandle = FindWindow(null, WindowTitle);
            if (windowHandle == IntPtr.Zero)
            {
                Debug.LogError("Window handle not found. Is 'Product Name' correct in Build Settings?");
                return;
            }

            // 1. 창 스타일 제거: 타이틀 바와 경계선 제거
            int currentStyle = GetWindowLong(windowHandle, GWL_STYLE);
            int newStyle = currentStyle & ~WS_CAPTION & ~WS_BORDER;
            SetWindowLong(windowHandle, GWL_STYLE, newStyle);

            // 2. 스타일 변경 적용 강제
            SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, 0, 0,
                        SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

            // 3. 초기 위치 및 크기 설정
            SetWindowPositionInternal(isExpanded);

            // 4. 전역 키 후킹 등록
            _proc = HookCallback;
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                IntPtr hModule = GetModuleHandle(curModule.ModuleName);
                hookId = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hModule, 0);
                if (hookId == IntPtr.Zero) Debug.LogError("Failed to register keyboard hook.");
            }
        }

        void UninitializeWindowAndHook()
        {
            // 후킹 해제 (반드시 필요)
            if (hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hookId);
                Debug.Log("Global Keyboard Hook unregistered.");
            }
        }
#endif
// ------------------------------------------------------------------------------------------
// 모든 플랫폼에서 컴파일 및 실행됨.
// ------------------------------------------------------------------------------------------
        public bool isExpanded = true;
        private const string WindowTitle = "Drill Game"; 

        [Header("Window Size (Percentage of Screen)")]
        [Range(0.01f, 1.0f)]
        public float ExpandedWidthPercent = 0.2f;
        [Range(0.01f, 1.0f)]
        public float ExpandedHeightPercent = 1f;

        [Range(0.01f, 1.0f)]
        public float CollapsedWidthPercent = 0.2f;
        [Range(0.01f, 1.0f)]
        public float CollapsedHeightPercent = 0.1f;
        
        // 💡 메인 스레드로 데이터를 전달하기 위한 큐 (모든 플랫폼에서 사용)
        private static Queue<int> keyEventQueue = new Queue<int>();
        private static object queueLock = new object();

        public int totalKeyPresses = 0;


        void Start()
        {
            // Windows 빌드에서만 네이티브 초기화 함수를 호출합니다.
            InitializeWindowAndHook();
        }

        /// <summary>
        /// 창 위치와 크기를 설정하는 Public 함수
        /// </summary>
        public void SetWindowPosition(bool expand)
        {
            Debug.Log("Window Toggle 호출");
            isExpanded = expand;
            SetWindowPositionInternal(expand); // 내부 함수 호출
        }

        /// <summary>
        /// 토글 버튼에 연결될 함수
        /// </summary>
        public void ToggleWindowSize()
        {
            SetWindowPosition(!isExpanded);
        }

        void Update()
        {
            
            // 명준 : UI 버튼 누르면 발동되게 바꿀게여
            //
            // 에디터에서도 테스트할 수 있도록 Alt+T 입력은 유지합니다.
            // if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            // {
            //     if (Input.GetKeyDown(KeyCode.T))
            //     {
            //         ToggleWindowSize();
            //     }
            // }

            // 💡 큐에서 키 입력 이벤트 처리 (메인 스레드 안전)
            lock (queueLock)
            {
                while (keyEventQueue.Count > 0)
                {
                    int vkCode = keyEventQueue.Dequeue();
                    totalKeyPresses++;
                    Debug.Log($"Key Pressed Globally: {vkCode}. Total Score: {totalKeyPresses}");

                    // 🚨 여기에 점수 증가 같은 게임 로직을 구현하면 됩니다.
                }
            }
        }

        void OnApplicationQuit()
        {
            // Windows 빌드에서만 후킹 해제 함수를 호출합니다.
            UninitializeWindowAndHook();
        }
    }
}