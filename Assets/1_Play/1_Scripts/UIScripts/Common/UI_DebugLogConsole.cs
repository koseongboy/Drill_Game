using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_DebugLogConsole : MonoBehaviour
    {
        // UI 요소
    public TextMeshProUGUI logText;
    public ScrollRect scrollRect;
    public int maxLogEntries = 50; // 최대 로그 개수

    // 로그 저장을 위한 리스트
    private List<string> logMessages = new List<string>();
    private bool isVisible = false; // 콘솔 창 표시/숨김 상태

    public void ToggleEnable()
    {
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }

    // 로그 타입별 색상 설정
    private Dictionary<LogType, string> logTypeColors = new Dictionary<LogType, string>()
    {
        { LogType.Log, "#FFFFFF" },       // 흰색 (기본 로그)
        { LogType.Warning, "#FFFF00" },   // 노란색
        { LogType.Error, "#FF0000" },     // 빨간색
        { LogType.Assert, "#FF00FF" },    // 마젠타
        { LogType.Exception, "#FF8800" }  // 주황색
    };

    void Start()
    {
        // 1. 애플리케이션의 로그 메시지 수신기에 등록
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        // 2. 스크립트가 파괴될 때 수신기 해제 (매우 중요)
        Application.logMessageReceived -= HandleLog;
    }

    // 로그 수신 시 호출되는 콜백 함수
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 로그 타입에 따라 색상 적용
        string colorCode = logTypeColors[type];
        string formattedLog = $"<color={colorCode}>[{type}] {logString}</color>";

        logMessages.Add(formattedLog);

        // 최대 로그 개수 초과 시 가장 오래된 로그 제거
        if (logMessages.Count > maxLogEntries)
        {
            logMessages.RemoveAt(0);
        }

        // UI 텍스트 업데이트
        UpdateLogText();
        
        // 콘솔이 활성화 상태일 경우에만 스크롤
        if (isVisible)
        {
             ScrollToBottom();
        }
    }

    // 로그 리스트를 하나의 문자열로 결합하여 UI에 표시
    void UpdateLogText()
    {
        // Join 대신 StringBuilder를 사용하면 더 효율적일 수 있습니다.
        logText.text = string.Join("\n", logMessages);
    }

    // 스크롤을 가장 아래(최신 로그)로 이동
    void ScrollToBottom()
    {
        // 다음 프레임에 실행되도록 지연
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }
    }
}