using System.Collections.Generic;
using DrillGame.Data;
using UnityEngine;

namespace DrillGame.Managers
{
    public class DataLoadManager : MonoBehaviour
    {
        #region Fields & Properties
        private Dictionary<string, Engine_Structure> engineTable;
        private Dictionary<string, Facility_Structure> facilityTable;
        private Dictionary<int, Ground_Structure> groundTable;
        private Dictionary<string, List<string>> userData; //임시!!!

        private GameObject groundPrefab;

        #endregion

        #region Singleton & initialization
        public static DataLoadManager Instance;
        #endregion

        #region getters & setters
        public Dictionary<string, Engine_Structure> GetEngineTable() { return engineTable; }
        public Dictionary<string, Facility_Structure> GetFacilityTable() { return facilityTable; }
        public Dictionary<int, Ground_Structure> GetGroundTable() { return groundTable; }
        public Dictionary<string, List<string>> GetUserData() { return userData; }
        public GameObject GetGroundPrefab() { return groundPrefab; }

        public void SetEngineTable(Dictionary<string, Engine_Structure> table) { engineTable = table; }
        public void SetFacilityTable(Dictionary<string, Facility_Structure> table) { facilityTable = table; }
        public void SetGroundTable(Dictionary<int, Ground_Structure> table) { groundTable = table; }
        public void SetUserData(Dictionary<string, List<string>> data) { userData = data; }
        public void SetGroundPrefab(GameObject prefab) { groundPrefab = prefab; }

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
        #endregion
    }
}
