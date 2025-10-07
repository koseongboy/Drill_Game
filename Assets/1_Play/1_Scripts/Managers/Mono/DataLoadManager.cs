using System.Collections.Generic;
using System.Threading.Tasks;
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
        private async Task<bool> LoadAllDataAsync()
        {
            var engine_Data = Engine_Data.CreateAsync();
            var facility_Data = Facility_Data.CreateAsync();
            var ground_Data = Ground_Data.CreateAsync();
            await Task.WhenAll(engine_Data, facility_Data, ground_Data);
            EngineTable = engine_Data.Result.EngineTable;
            FacilityTable = facility_Data.Result.FacilityTable;
            GroundTable = ground_Data.Result.GroundTable;
            return EngineTable != null && FacilityTable != null && GroundTable != null;
        }
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
                transform.parent = null;
                DontDestroyOnLoad(this.gameObject);
            }
            Debug.Log("DataLoadManager Awake completed.");
        }
        async void Start()
        {
            if (await LoadAllDataAsync())
            {
                Debug.Log("All CSV_Data has been loaded successfully.");
            }
            else
            {
                Debug.LogError("Failed to load some CSV_Data.");
            }
        }
        #endregion
    }
}
