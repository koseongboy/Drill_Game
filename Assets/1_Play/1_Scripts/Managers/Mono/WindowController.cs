using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


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
        private static extern IntPtr SetWindowsHookEx(int idHook, IntPtr lpfn, IntPtr hMod, uint dwThreadId);

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

        private const int WH_MOUSE_LL = 14; 

        // 마우스 메시지 상수
        private const int WM_LBUTTONDOWN = 0x0201; // 마우스 왼쪽 버튼 클릭
        private const int WM_RBUTTONDOWN = 0x0204; // 마우스 오른쪽 버튼 클릭

        private IntPtr mouseHookId = IntPtr.Zero;
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelMouseProc _mouseProc;

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

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

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

        // 💡 마우스 입력 이벤트 큐 (메시지 타입, X좌표, Y좌표 저장)

        private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int msg = wParam.ToInt32();

                // 💡 좌클릭(0x0201) 또는 우클릭(0x0204)만 처리
                if (msg == WM_LBUTTONDOWN || msg == WM_RBUTTONDOWN) 
                {
                    
                    // 큐에 메시지 타입, X좌표, Y좌표를 저장
                    lock (queueLock) 
                    {
                        keyEventQueue.Enqueue(msg);
                    }
                }
            }

            // CallNextHookEx를 호출하여 메시지를 다른 프로그램으로 투과
            return CallNextHookEx(mouseHookId, nCode, wParam, lParam);
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
            float heightPercent = expand ? ExpandedHeightPercent : CollapsedHeight;

            int newWidth = (int)(screenWidth * widthPercent);
            // 확장 시 상하단 45픽셀씩 여유를 두거나 60px 크기로 설정
            int newHeight = expand ? (int)(screenHeight * heightPercent * 0.91)  : CollapsedHeight; 

            // X 계산: (화면 오른쪽 끝) - (새로운 창 너비)
            int newX = screenWidth - newWidth;

            // Y 좌표 (오른쪽 상단 고정: Y=0)
            int newY = (int)(screenHeight * 0.955) - newHeight;

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
                Debug.LogError("창 이름 변수 수정할 것. 유니티 에디터일 경우 무시해도 됨.");
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
            IntPtr keyboardProcPtr = Marshal.GetFunctionPointerForDelegate(_proc); // 키보드 델리게이트 변환
            _mouseProc = MouseHookCallback;
            IntPtr mouseProcPtr = Marshal.GetFunctionPointerForDelegate(_mouseProc); // 마우스 델리게이트 변환
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                IntPtr hModule = GetModuleHandle(curModule.ModuleName);
                hookId = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardProcPtr, hModule, 0);
                mouseHookId = SetWindowsHookEx(WH_MOUSE_LL, mouseProcPtr, hModule, 0);
                if (hookId == IntPtr.Zero) Debug.LogError("Failed to register keyboard hook.");
                if (mouseHookId == IntPtr.Zero) Debug.LogError("Failed to register mouse hook.");
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

        [SerializeField] private Canvas mainCanvas;
        RectTransform canvasRectTransform;
        [SerializeField] private RectTransform targetRect;

    // 평상시(축소 상태)의 여백/오프셋 값
        [Header("축소 상태 오프셋")]
        [SerializeField] private float normalTop = 100f; 
        [SerializeField] private float normalBottom = 100f;

        [Header("Window Size (Percentage of Screen)")]
        [Range(0.01f, 1.0f)]
        public float ExpandedWidthPercent = 0.2f;
        [Range(0.01f, 1.0f)]
        public float ExpandedHeightPercent = 1f;

        [Range(0.01f, 1.0f)]
        public float CollapsedWidthPercent = 0.2f;
        [Range(10, 100)]
        public int CollapsedHeight = 40;

        Vector2 originalSizetMax;
        Vector2 originalSizeMin;
        
        // 💡 메인 스레드로 데이터를 전달하기 위한 큐 (모든 플랫폼에서 사용)
        private static Queue<int> keyEventQueue = new Queue<int>();
        private static object queueLock = new object();

        public int totalKeyPresses = 0;


        void Start()
        {
            
            SetWindowPositionInternal(isExpanded);
        }

        void Awake()
        {          
            canvasRectTransform = targetRect.root.GetComponent<RectTransform>();
            InitializeWindowAndHook();
        }
        /// <summary>
        /// 창 위치와 크기를 설정하는 Public 함수
        /// </summary>
        public void ToggleWindowSize()
        {
            isExpanded = !isExpanded;
            mainCanvas.enabled = isExpanded;
            SetWindowPositionInternal(isExpanded); // 내부 함수 호출
            if (isExpanded)
            {
                targetRect.anchorMin = new Vector2(0f, 0f);
                targetRect.anchorMax = new Vector2(1f, 0f);
                targetRect.anchoredPosition = new Vector2(0f, 45f);
                targetRect.sizeDelta = new Vector2(0f, 90f);
                
            }
            else
            {
                targetRect.anchorMin = new Vector2(0f, 0f);
                targetRect.anchorMax = new Vector2(1f, 1f);

                targetRect.anchoredPosition = Vector2.zero;
                targetRect.sizeDelta = Vector2.zero;

            }
        }
        private void SetOffsets(float top, float bottom, float left, float right)
        {
            targetRect.offsetMin = new Vector2(left, bottom);

            targetRect.offsetMax = new Vector2(-right, -top);
        }

        void Update()
        {
            lock (queueLock)
            {
                while (keyEventQueue.Count > 0)
                {
                    keyEventQueue.Dequeue();
                    InputCountManager.Instance.addInputCount();
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