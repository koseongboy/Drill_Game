using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DrillGame.Core.Managers;

namespace DrillGame.Core.Facility
{
    public class FacilityEntity
    {
        #region Fields & Properties
        private Vector2Int position; // 시설의 위치 (중점)
        private List<Vector2Int> formations = new List<Vector2Int>(); // 시설의 형태 (중점 기준 상대 좌표 리스트) , 0,0 필수

        public event Action OnFacilityActivated;
        #endregion

        #region Singleton & initialization
        public FacilityEntity(Vector2Int startPosition, List<Vector2Int> formations = null)
        {
            position = startPosition;
            // for test
            if (formations == null)
            {
                this.formations.Add(new Vector2Int(0, 0));
            }
            else
            {
                this.formations = formations;
            }

            // register to BoardManager
            BoardManager.Instance.AddFacility(this);
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
        public void Run()
        {
            Debug.Log("Facility is running.");
            OnFacilityActivated?.Invoke();
        }
        public void ShowFacilityInfo()
        {
            Debug.LogError("시설 UI를 띄워주세요!");
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
