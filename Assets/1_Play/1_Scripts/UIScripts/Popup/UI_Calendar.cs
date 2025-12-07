using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using DrillGame.Managers;
using DrillGame.UI.Interface;

namespace DrillGame
{
    public class UI_Calendar : MonoBehaviour, UI_IAddressable
    {
        #region Singleton & initialization
        [SerializeField] private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        #endregion
        
        private DateTime _currentDate = DateTime.Today;
        
        private VisualElement _selectedDayCell = null;

        private VisualElement _dateGrid;
        private Label _monthYearLabel;
        private Button _prevMonthButton;
        private Button _nextMonthButton;
        
        // 최대 플레이 시간
        private const float MaxPlayTimeForHeatmap = 24.0f;
        // 연한 녹색 (시작 색상)
        private readonly Color LightGreen = new Color(0.8f, 1.0f, 0.8f, 1.0f); // R, G, B, A (204, 255, 204)
        // 진한 녹색 (끝 색상)
        private readonly Color DarkGreen = new Color(0.1f, 0.4f, 0.1f, 1.0f); // R, G, B, A (25, 102, 25)
        

        // MonoBehaviour가 활성화될 때 실행됩니다.
        private void OnEnable()
        {
            // 1. UIDocument 컴포넌트 참조
            var uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null) return;

            // 2. Visual Tree의 Root 요소 가져오기
            var root = uiDocument.rootVisualElement;

            // 3. UXML 요소들을 이름(Name)을 사용하여 Query (Q)로 찾습니다.
            _dateGrid = root.Q<VisualElement>("Calendar_DateGrid");
            _monthYearLabel = root.Q<Label>("MonthYear_Label");
            _prevMonthButton = root.Q<Button>("PrevMonth_Button");
            _nextMonthButton = root.Q<Button>("NextMonth_Button");

            // 4. 월 이동 버튼에 이벤트 연결 (5단계에서 자세히 구현)
            _prevMonthButton.clicked += () => ChangeMonth(-1);
            _nextMonthButton.clicked += () => ChangeMonth(1);

            // 5. 달력 생성 함수 호출
            GenerateCalendar();
        }

        // 다음 월/이전 월로 이동하는 함수
        private void ChangeMonth(int change)
        {
            _currentDate = _currentDate.AddMonths(change);
            GenerateCalendar();
        }

        // 달력 UI를 동적으로 생성하는 핵심 함수
        private void GenerateCalendar()
        {
            _monthYearLabel.text = _currentDate.ToString("yyyy년 MM월");
            _dateGrid.Clear(); 

            // 1. 해당 월의 첫 번째 날짜와 요일을 구합니다.
            DateTime firstDayOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(_currentDate.Year, _currentDate.Month);
            
            // 첫 번째 날이 무슨 요일인지 (0=일요일, 1=월요일, ... 6=토요일)
            // C# DayOfWeek는 Sunday=0, Monday=1 입니다.
            int startingDayOfWeek = (int)firstDayOfMonth.DayOfWeek; 
            int offset = (startingDayOfWeek - 1 + 7) % 7; // 월요일(1)이 0이 되도록 조정

            // 2. 빈 셀 추가 (첫째 날 이전의 빈 칸)
            for (int i = 0; i < offset; i++)
            {
                _dateGrid.Add(CreateEmptyDayCell());
            }

            // 3. 해당 월의 날짜 셀 생성
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(_currentDate.Year, _currentDate.Month, day);
                VisualElement cell = CreateDayCell(day, date);
                
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    cell.AddToClassList("calendar-saturday-cell"); 
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    cell.AddToClassList("calendar-sunday-cell"); 
                }
                
                _dateGrid.Add(CreateDayCell(day, date));
            }

            // 4. 마지막 빈 셀 추가 (총 42개 셀을 채우기 위해)
            while (_dateGrid.childCount < 42)
            {
                _dateGrid.Add(CreateEmptyDayCell());
            }
        }
        
        // 빈 날짜 셀을 생성합니다.
        private VisualElement CreateEmptyDayCell()
        {
            VisualElement cell = new VisualElement();
            cell.AddToClassList("day-cell"); // 스타일링을 위한 클래스 추가 (USS에 정의 필요)
            return cell;
        }

        // 날짜가 표시된 셀을 생성하고 클릭 이벤트를 연결합니다.
        private VisualElement CreateDayCell(int day, DateTime date)
        {
            VisualElement cell = new VisualElement();
            cell.AddToClassList("day-cell"); 
    
            // 1. 날짜 Label
            Label dayLabel = new Label(day.ToString());
            dayLabel.AddToClassList("day-number-label");
            cell.Add(dayLabel);
    
            // 2. 플레이 시간 Label 추가
            float playTime = GetPlayTimeForDate(date);
            Label timeLabel = new Label();
            timeLabel.AddToClassList("play-time-label");
            
            // 3. 입력횟수 Label 추가
            int inputCount = GetInputCountForDate(date);
            Label playTimeLabel = new Label();
            playTimeLabel.AddToClassList("input-count-label");

            if (playTime > 0)
            {
                int hours = (int)playTime;
                int minutes = (int)((playTime - hours) * 60);
                timeLabel.text = $"{hours}h {minutes}m";

                playTimeLabel.text = inputCount.ToString();
                
                float normalizedPlayTime = Mathf.Clamp01(playTime / MaxPlayTimeForHeatmap);
                Color heatMapColor = Color.Lerp(LightGreen, DarkGreen, normalizedPlayTime);
                cell.style.backgroundColor = heatMapColor;

                if (normalizedPlayTime > 0.3f)
                {
                    dayLabel.style.color = Color.white; 
                    timeLabel.style.color = Color.white;
                    playTimeLabel.style.color = Color.white;
                }
            }
            else
            {
                // 플레이 시간이 0이면 비워두거나, 특정 텍스트를 표시합니다.
                timeLabel.text = ""; 
                playTimeLabel.text = "";
            }
            cell.Add(timeLabel); // 날짜 셀에 시간 Label 추가
            cell.Add(playTimeLabel); // 날짜 셀에 입력 Label 추가

            return cell;
        }
        
        
        private float GetPlayTimeForDate(DateTime date)
        {
            // 1. SaveManager 인스턴스 확인
            if (SaveManager.Instance == null)
            {
                Debug.LogError("SaveManager 인스턴스를 찾을 수 없습니다.");
                return 0; 
            }

            // 2. 전체 일별 플레이 시간 데이터 로드
            Dictionary<string, int> playTimeData = 
                SaveManager.Instance.LoadAllDailyPlayTimeData(
                    new Dictionary<string, int>() // 기본값 (저장된 데이터가 없을 경우)
                );

            // 3. 요청된 날짜의 키 생성 ("yyyy-MM-dd" 형식)
            string dateKey = date.ToString("yyyy-MM-dd");
            int totalPlayTimeInSeconds = 0;

            // 4. 해당 날짜의 시간(초) 조회
            playTimeData.TryGetValue(dateKey, out totalPlayTimeInSeconds);
    
            // 5. 초를 시간 단위(float)로 변환하여 반환
            // 1시간 = 3600초
            if (totalPlayTimeInSeconds > 0)
            {
                // 시간을 float 형식으로 반환합니다. (예: 3.5f)
                return totalPlayTimeInSeconds / 3600f;
            }

            return 0f; // 해당 날짜에 저장된 데이터가 없으면 0시간 반환
        }

        private int GetInputCountForDate(DateTime date)
        {
            // 1. SaveManager 인스턴스 확인
            if (SaveManager.Instance == null)
            {
                Debug.LogError("SaveManager 인스턴스를 찾을 수 없습니다.");
                return 0; 
            }

            // 2. 전체 일별 플레이 시간 데이터 로드
            Dictionary<string, int> inputCountData = 
                SaveManager.Instance.LoadAllInputCountData(
                    new Dictionary<string, int>() // 기본값 (저장된 데이터가 없을 경우)
                );

            // 3. 요청된 날짜의 키 생성 ("yyyy-MM-dd" 형식)
            string dateKey = date.ToString("yyyy-MM-dd");
            int totalInputCount = 0;

            // 4. 해당 날짜의 시간(초) 조회
            inputCountData.TryGetValue(dateKey, out totalInputCount);
            return totalInputCount;
        }
        
    }
}