using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DrillGame.Core.Ground;
using DrillGame.View.Ground;
using DrillGame.Core.Managers;

namespace DrillGame.Core.Facility
{
    public class FacilityEntity
    {
        #region Fields & Properties

        public Facility_Data_ data;
        protected int facilityId;
        protected int itemId;
        protected Vector2Int position; // 시설의 위치 (중점)
        protected List<Vector2Int> formations = new List<Vector2Int>(); // 시설의 형태 (중점 기준 상대 좌표 리스트) , 0,0 필수

        public event Action<int> OnFacilityActivated;

        public event Action OnFacilityDeleted;

        protected int Level;
        #endregion

        #region Singleton & initialization
        public FacilityEntity(Vector2Int startPosition, int id)
        {
            data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(id);
            this.position = startPosition;
            // for test
            if (formations == null)
            {
                this.formations.Add(new Vector2Int(0, 0));
            }
            else
            {
                this.formations = data.GetCoordinates();
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

        public List<Vector2Int> GetFormations()
        {
            return formations;
        }

        public int GetFacilityId()
        {
            return facilityId;
        }
        #endregion

        #region public methods
        public virtual void Run(int intensity)
        {
            Debug.Log("Facility is running. with Intensity : "  + intensity);

            // 시설 고유의 액션 실행
            for (int i = 0; i < intensity; i++)
            {
                Logger("Hello from Facility! Intensity: " + intensity);
            }

            // 이벤트 호출 (presenter -> component)
            OnFacilityActivated?.Invoke(intensity);
        }
        

        public void DeleteEntity()
        {
            // presentor에게 알림
            OnFacilityDeleted?.Invoke();

            // BoardManager에서 제거
            BoardManager.Instance.RemoveFacility(this);
            
            // 인벤토리에 아이템 추가
            InventoryManager.Instance.AddItemById(itemId);
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
