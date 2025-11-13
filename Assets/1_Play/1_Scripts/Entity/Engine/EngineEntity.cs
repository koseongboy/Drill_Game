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

        private bool isRunning = true; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ìµï¿½ï¿½ï¿½Å³ï¿½ï¿½ falseï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        private List<int> scheduleList = new List<int>(); // ï¿½ï¿½ï¿½ï¿½ Æ½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½ï¿½ï¿½ï¿½Æ®

        private Vector2Int position; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ (ï¿½ï¿½ï¿½ï¿½)
        private List<Vector2Int> formations = new List<Vector2Int>(); // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ (ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿? ï¿½ï¿½Ç¥ ï¿½ï¿½ï¿½ï¿½Æ®) , 0,0 ï¿½Ê¼ï¿½  


        public event Action OnEngineActivated;

        public event Action OnEngineDeleted;

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
        public void DeleteEntity()
        {
            // presentor¿¡°Ô ¾Ë¸²
            OnEngineDeleted?.Invoke();
            // BoardManager¿¡¼­ ÀÚ½ÅÀ» Á¦°Å
            BoardManager.Instance.RemoveEngine(this);
            
        }
        public void Tick()
        {
            if (!isRunning) return;  // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ö´Ù¸ï¿½ Æ½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½

            ScheduleTick();
        }

        // for test
        public void ScheduleEngineRun(int tickCount)
        {
            scheduleList.Add(tickCount);

        }

        public void ScheduleEngineRun(Vector2Int corePosition)
        {
            // ï¿½ï¿½ï¿½ï¿½Æ° ï¿½Å¸ï¿½ ï¿½ï¿½Å­ï¿½ï¿½ Æ½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Õ´Ï´ï¿?.
            int distance = Mathf.Abs(corePosition.x - position.x) + Mathf.Abs(corePosition.y - position.y);

            scheduleList.Add(distance);
        }

        public void ShowEngineInfo()
        {
            // ÀÓ½Ã·Î »èÁ¦ ±¸Çö
            DeleteEntity();

            Debug.LogError("¿£Áø UI¸¦ ¶ç¿öÁÖ¼¼¿ä!");
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
                    // ï¿½ï¿½ï¿? ï¿½ï¿½ï¿½ï¿½
                    ActivateEngine();
                    scheduleList.RemoveAt(i);
                }

            }
        }
        private void ActivateEngine()
        {
            Debug.Log($"Engine at {position} activated!");
            // ï¿½ï¿½ï¿½â¿¡ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ È°ï¿½ï¿½È­ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Õ´Ï´ï¿½.
            OnEngineActivated?.Invoke();
            
            BoardManager.Instance.RegisterRun(GetFormationPositions());
        }
        
        #endregion

        #region Unity event methods
        #endregion


    }

    
}
