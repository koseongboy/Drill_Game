using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DrillGame.Core.Managers;
using DrillGame.Managers;
using DrillGame.UI;

namespace DrillGame.Core.Engine
{
    public class EngineEntity : IEntityHandler
    {
        #region Fields & Properties

        [ReadOnly]
        [SerializeField]
        private string engineId; // UI 표시용 Data Id값
        
        private bool isRunning = true; // ������ �̵���ų�� false�� ����
        private List<int> scheduleList = new List<int>(); // ���� ƽ ���� �����ϴ� ����Ʈ

        private Vector2Int position; // ������ ��ġ (����)
        private List<Vector2Int> formations = new List<Vector2Int>(); // ������ ���� (���� ���� ��� ��ǥ ����Ʈ) , 0,0 �ʼ�  
        
        public event Action OnEngineActivated;
        public event Action OnEngineDeleted;

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

            // register to BoardManager
            BoardManager.Instance.AddEngine(this);
        }
        #endregion

        #region getters & setters

        public void SetEngineId(string Id)
        {
            engineId = Id;
        }

        public string GetEngineId()
        {
            return engineId;
        }
        
        
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
        public void DeleteEntity()
        {
            // presentor 호출
            OnEngineDeleted?.Invoke();
            // BoardManager 에서 제거
            BoardManager.Instance.RemoveEngine(this);
            
            // TODO : 인벤토리에 아이템 추가
        }

        public void MoveEntity()
        {
            // delete 코드 사용후 다시 집어드는 판정입니다.
            OnEngineDeleted?.Invoke();
            BoardManager.Instance.RemoveEngine(this);
            
            // Todo : 적합한 id 필요
            GameManager.Instance.SetBatchEntity(1);

            GameManager.Instance.StartBatch();
        }

        public void Tick()
        {
            if (!isRunning) return;  // 실행 중이 아니라면 무시

            ScheduleTick();
        }

        // for test
        public void ScheduleEngineRun(int tickCount)
        {
            scheduleList.Add(tickCount);
        }

        public void ScheduleEngineRun(Vector2Int corePosition)
        {
            // ����ư �Ÿ� ��ŭ�� ƽ�� ����մϴ�.
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
                    // ��� ����
                    ActivateEngine();
                    scheduleList.RemoveAt(i);
                }

            }
        }
        private void ActivateEngine()
        {
            Debug.Log($"Engine at {position} activated!");
            // ���⿡ ������ Ȱ��ȭ�� ���� ������ �����մϴ�.
            OnEngineActivated?.Invoke();
            
            BoardManager.Instance.RegisterRun(GetFormationPositions());
        }
        
        #endregion

        #region Unity event methods
        #endregion


    }

    
}
