using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using DrillGame.Core.Engine;
using DrillGame.Core.Facility;


namespace DrillGame.Core.Managers
{
    public class BoardManager
    {
        #region Fields & Properties
        public static BoardManager instance;

        private List<EngineEntity> engines;
        private List<FacilityEntity> facilities;
        private Dictionary<Vector2Int, FacilityEntity> facilityMap;

        private Dictionary<FacilityEntity, int> ScheduledFacilities;

        private Vector2Int corePosition;

        private int tickCount;
        private const int TICK_INTERVAL = 10;
        #endregion

        #region Singleton & initialization
        public static BoardManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BoardManager();
                }
                return instance;
            }
        }
        private BoardManager()
        {
            engines = new();
            facilities = new();
            facilityMap = new();
            ScheduledFacilities = new();

            corePosition = Vector2Int.zero;
            tickCount = TICK_INTERVAL - 1;

            // for test
            //AddEngine(new EngineEntity(new Vector2Int(0, 2)));
            //List<Vector2Int> formation = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(0, -1) };
            //AddFacility(new FacilityEntity(new Vector2Int(0, 3), formation));
        }
        #endregion

        #region getters & setters
        public void SetCorePosition(Vector2Int position)
        {
            corePosition = position;
        }
        #endregion

        #region public methods
        public void Tick()
        {
            Debug.Log("Engine Core Tick");
            TickAllEngines();       // 모든 엔진의 틱을 선행     (여기서 시설의 등록이 이루어짐)
            ProcessTickCycle();     // 이후 코어 틱 진행 -> 명령 실행 직후 1틱이 흘러가는 것을 방지 (주로 명령 단계)
            RunFacility();            // 모든 엔진에 명령 실행
        }

        public void AddEngine(EngineEntity engine)
        {
            engines.Add(engine);
        }

        public void RemoveEngine(EngineEntity engine)
        {
            engines.Remove(engine);
        }

        
        public void AddFacility(FacilityEntity facility)
        {
            facilities.Add(facility);
            // facilityMap에 추가하는 로직 추가 필요


            // 정상적인 입력만 들어온다고 가정
            List<Vector2Int> positions = facility.GetFormationPositions();
            foreach (var pos in positions)
            {
                facilityMap[pos] = facility;
            }
        }
        public void RemoveFacility(FacilityEntity facility)
        {
            facilities.Remove(facility);
            // facilityMap에서 제거하는 로직 추가 필요

            List<Vector2Int> positions = facility.GetFormationPositions();
            foreach (var pos in positions)
            {
                facilityMap.Remove(pos);
            }
        }

        public void RegisterRun(List<Vector2Int> positions) 
        {
            foreach (var pos in positions)
            {
                if (facilityMap.TryGetValue(pos, out FacilityEntity facility))
                {
                    if (!ScheduledFacilities.ContainsKey(facility))
                    {
                        ScheduledFacilities[facility] = 1;
                    }
                    else
                    {
                        ScheduledFacilities[facility]++;
                    }
                }
            }

        }
        #endregion

        #region private methods
        private void TickAllEngines()
        {
            foreach (var engine in engines)
            {
                engine.Tick();
            }
        }

        private void ProcessTickCycle()
        {
            tickCount++;
            if (tickCount >= TICK_INTERVAL)
            {
                tickCount = 0;
                ActivateCore();
            }
        }
        private void RunFacility()
        {
            foreach (var facility in ScheduledFacilities)
            {
                facility.Key.Run(facility.Value);
            }
            ScheduledFacilities.Clear();
        }

        // 코어가 활성화 되면 모든 엔진에 명령을 내립니다.
        private void ActivateCore()
        {
            Debug.Log($"Engine Core Activated : have {engines.Count} engines & {facilities.Count} facilities");
            foreach (var engine in engines)
            {
                engine.ScheduleEngineRun(corePosition);
            }
        }
        #endregion

    }
}
