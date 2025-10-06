using System.Collections.Generic;
using DrillGame.Data;
using UnityEngine;

namespace DrillGame.Managers
{
    public class DataLoadManager : MonoBehaviour
    {
        #region Fields & Properties
        public Dictionary<string, Engine_Structure> EngineTable { get; set; }
        public Dictionary<string, Facility_Structure> FacilityTable { get; set; }
        public Dictionary<int, Ground_Structure> GroundTable { get; set; }
        public Dictionary<string, List<string>> UserData {get; set;} //임시!!!'

        #endregion

        #region Singleton & initialization
        public static DataLoadManager Instance;
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            Debug.Log("DataLoadManager Awake completed.");
        }
        async void Start()
        {
            var engine_Data = await Engine_Data.CreateAsync();
            var facility_Data = await Facility_Data.CreateAsync();
            var ground_Data = await Ground_Data.CreateAsync();
            Debug.Log("All CSV_Data has been awaked.");
            EngineTable = engine_Data.EngineTable;
            FacilityTable = facility_Data.FacilityTable;
            GroundTable = ground_Data.GroundTable;
            Debug.Log("DataLoadManager tables set.");

        }
        #endregion
    }
}
