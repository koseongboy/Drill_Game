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
        private int itemId; // 철거할 때 인벤토리에 넣기 위한 ItemId값
        
        [ReadOnly]    
        private int engineId; // UI 표시용 Data Id값
        
        private bool isRunning = true; // ?????? ???????? false?? ????
        private List<int> scheduleList = new List<int>(); // ???? ? ???? ??????? ?????

        private Vector2Int position; // ?????? ??? (????)
        private List<Vector2Int> formations = new List<Vector2Int>(); // ?????? ???? (???? ???? ??? ??? ?????) , 0,0 ???  
        
        public event Action OnEngineActivated;
        public event Action OnEngineDeleted;
        public event Action<int> OnEngineTickScheduled;

        #endregion

        #region Singleton & initialization
        public EngineEntity(Vector2Int startPosition, List<Vector2Int> formations, int itemId, int engineId, string type)
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
            
            this.itemId = itemId;
            this.engineId = engineId;

            // register to BoardManager
            BoardManager.Instance.AddEngine(this);
        }
        #endregion

        #region getters & setters
        public void SetEngineItemId(int Id)
        {
            itemId = Id;
        }

        public int GetEngineItemId()
        {
            return itemId;
        }
        
        public void SetEngineId(int Id)
        {
            engineId = Id;
        }

        public int GetEngineId()
        {
            return engineId;
        }
        public bool IsCore()
        {
            if (engineId == 210001) return true;
            return false;
        }

        public Vector2Int GetPosition()
        {
            return position;
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
            // 인벤토리에 아이템 추가
            InventoryManager.Instance.AddItem(itemId);
        }

        public void MoveEntity()
        {
            // delete 코드 사용후 다시 집어드는 판정입니다.
            OnEngineDeleted?.Invoke();
            BoardManager.Instance.RemoveEngine(this);
            GameManager.Instance.BatchEntity(itemId);
        }

        public void Tick()
        {
            if (!isRunning) return;  // 실행 중이 아니라면 무시
            //if (engineId == 210001) Debug.Log("저는 코어에요");
            ScheduleTick();
        }

        // for test
        public void ScheduleEngineRun(int tickCount)
        {
            scheduleList.Add(tickCount);
        }

        public void ScheduleEngineRun(Vector2Int corePosition)
        {
            // 맨해튼 거리 계산
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
                OnEngineTickScheduled?.Invoke(scheduleList[i]);
                if (scheduleList[i] <= 0)
                {
                    // ??? ????
                    ActivateEngine();
                    scheduleList.RemoveAt(i);
                }

            }
        }
        private void ActivateEngine()
        {
            // Debug.Log($"Engine at {position} activated!");
            // ???? ?????? ?????? ???? ?????? ????????.
            OnEngineActivated?.Invoke();
            
            BoardManager.Instance.RegisterRun(GetFormationPositions());
        }
        
        #endregion

        #region Unity event methods
        #endregion


    }

    
}
