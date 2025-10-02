using DrillGame.Entity.Engine;
using DrillGame.Entity.Facility;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Managers
{
    public class BoardManager : MonoBehaviour
    {
        #region Fields & Properties
        private Dictionary<Vector2Int, Engine_Base> enginePositions = new();
        private Dictionary<Vector2Int, Facility_Base> facilityPositions = new();

        #endregion

        #region Singleton & initialization
        public static BoardManager Instance { get; private set; }


        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void EngineAction(List<Vector2Int> targetPositions, string name)
        {
            Debug.Log($"{name} 이 action 실행. 대상 위치 : {string.Join(", ", targetPositions)}");
            
            List<Facility_Base> affectedFacilities = new List<Facility_Base>();
            foreach (var pos in targetPositions)
            {
                if (facilityPositions.TryGetValue(pos, out Facility_Base facility))
                {
                    if (!affectedFacilities.Contains(facility))
                    {
                        affectedFacilities.Add(facility);
                    }
                }
            }

            foreach (var facility in affectedFacilities)
            {
                Debug.Log($"{name} 이 {facility.GetEntityName()} 시설을 작동시킵니다.");
                facility.ActivateFacility();
            }
        }
        #endregion

        #region public methods - registration
        public void RegisterEngine(Engine_Base engine)
        {
            List<Vector2Int> positions = engine.GetAllPositions();

            // todo 체크 검사 필요
            foreach (var pos in positions)
            {
                enginePositions[pos] = engine;
            }
        }

        public void RegisterFacility(Facility_Base facility)
        {
            List<Vector2Int> positions = facility.GetAllPositions();

            // todo 체크 검사 필요
            foreach (var pos in positions)
            {
                facilityPositions[pos] = facility;
            }

        }

        public void UnregisterEngine(Engine_Base engine)
        {

        }
        public void UnregisterFacility(Facility_Base facility)
        {

        }


        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }
        #endregion
    }
}
