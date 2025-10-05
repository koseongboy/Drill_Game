using UnityEngine;
using DrillGame.Data;
using System;
using System.Collections.Generic;


namespace DrillGame.Managers
{
    public class CSVLoader : MonoBehaviour
    {
        #region Fields & Properties
            private Engine_Data engineData;
            private Facility_Data facilityData;
            private Ground_Data groundData;
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        public Engine_Data GetEngineData() { return engineData; }
            public Facility_Data GetFacilityData() { return facilityData; }
            public List<Tuple<int, int>> GetEngineCoordinates(string id) { return engineData.GetCoordinate(id); }
            public Facility_Structure GetFacilityStructure(string id) { return facilityData.GetFacility_Structure(id); }
        #endregion

        #region public methods
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        async void Awake()
        {
            engineData = await Engine_Data.CreateAsync();
            facilityData = await Facility_Data.CreateAsync();
            groundData = await Ground_Data.CreateAsync();
            Debug.Log("CSVLoader Awake completed.");
        }

        
        void Update()
        {
        
        }
        #endregion

    }
}
