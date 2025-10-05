using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DrillGame.Core.Managers;

namespace DrillGame.Core.Engine
{
    public class EngineEntity
    {
        #region Fields & Properties

        private bool isRunning = true; // 엔진을 이동시킬땐 false로 변경
        private List<int> scheduleList = new List<int>(); // 남은 틱 수를 저장하는 리스트

        private Vector2Int position; // 엔진의 위치 (중점)
        private List<Vector2Int> formations = new List<Vector2Int>(); // 엔진의 형태 (중점 기준 상대 좌표 리스트) , 0,0 필수  


        #endregion

        #region Singleton & initialization
        public EngineEntity(Vector2Int startPosition, List<Vector2Int> formations = null)
        {
            position = startPosition;
            if (formations == null)
            {
                this.formations.Add(new Vector2Int(0, 0));
            }
            else
            {
                this.formations = formations;
            }
        }
        #endregion

        #region getters & setters
        public List<Vector2Int> GetFormationPositions()
        {
            List<Vector2Int> absolutePositions = new List<Vector2Int>();
            foreach (var formation in formations)
            {
                absolutePositions.Add(new Vector2Int(position.x + formation.x, position.y + formation.y));
            }
            return absolutePositions;
        }
        #endregion

        #region public methods
        public void Tick()
        {
            if (!isRunning) return;  // 엔진이 멈춰있다면 틱을 진행하지 않음

            ScheduleTick();
        }

        // for test
        public void ScheduleEngineRun(int tickCount)
        {
            scheduleList.Add(tickCount);

        }

        public void ScheduleEngineRun(Vector2Int corePosition)
        {
            // 맨허튼 거리 만큼의 틱을 사용합니다.
            int distance = Mathf.Abs(corePosition.x - position.x) + Mathf.Abs(corePosition.y - position.y);

            scheduleList.Add(distance);
        }

        

        #endregion

        #region private methods
        private void ScheduleTick()
        {
            for (int i = scheduleList.Count - 1; i >= 0; i--)
            {
                scheduleList[i] -= 1;
                if (scheduleList[i] <= 0)
                {
                    // 명령 실행
                    ActivateEngine();
                    scheduleList.RemoveAt(i);
                }

            }
        }
        private void ActivateEngine()
        {
            Debug.Log($"Engine at {position} activated!");
            // 여기에 엔진이 활성화될 때의 동작을 구현합니다.
            BoardManager.Instance.RegisterEngineRun(GetFormationPositions());
        }
        
        #endregion

        #region Unity event methods
        #endregion


    }

    
}
