using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DrillGame.Core.Managers;
using DrillGame.UI;

namespace DrillGame.Core.Engine
{
    public class EngineEntity
    {
        #region Fields & Properties

        private bool isRunning = true; // ������ �̵���ų�� false�� ����
        private List<int> scheduleList = new List<int>(); // ���� ƽ ���� �����ϴ� ����Ʈ

        private Vector2Int position; // ������ ��ġ (����)
        private List<Vector2Int> formations = new List<Vector2Int>(); // ������ ���� (���� ���� ��� ��ǥ ����Ʈ) , 0,0 �ʼ�  


        public event Action OnEngineActivated;

        #endregion

        #region Singleton & initialization
        public EngineEntity(Vector2Int startPosition, List<Vector2Int> formations = null)
        {
            this.position = startPosition;
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
            if (!isRunning) return;  // ������ �����ִٸ� ƽ�� �������� ����

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

        public void ShowEngineInfo()
        {
            // Debug.LogError("���� UI�� ����ּ���!");
            UILoader.Instance.ShowUI("UI_CoreDetailPopup");
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
