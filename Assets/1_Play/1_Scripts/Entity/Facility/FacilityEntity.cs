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

        public event Action<int> OnFacilityActivated;

        public event Action OnFacilityDeleted;

        private IFacilityAction facilityAction;
        #endregion

        #region Singleton & initialization
        public FacilityEntity(Vector2Int startPosition, List<Vector2Int> formations, IFacilityAction facilityAction)
        {
            this.position = startPosition;
            this.facilityAction = facilityAction;
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
        public void Run(int intensity)
        {
            Debug.Log("Facility is running. with Intensity : "  + intensity);

            // 시설 고유의 액션 실행
            facilityAction?.ActivateFacility(this, intensity);

            // 이벤트 호출 (presenter -> component)
            OnFacilityActivated?.Invoke(intensity);
        }
        

        public void DeleteEntity()
        {
            // presentor에게 알림
            OnFacilityDeleted?.Invoke();

            // BoardManager에서 제거
            BoardManager.Instance.RemoveFacility(this);
        }

        // 여기서 부터 model 관련 메서드 추가 가능
        public void Logger(string message)
        {
            Debug.Log(message);
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
