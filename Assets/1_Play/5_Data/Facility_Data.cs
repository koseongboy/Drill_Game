using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal.Internal;
using System;


namespace DrillGame.Data
{
    public class Facility_Data: CSV_Data
    {
        #region Fields & Properties
        #endregion

        #region Singleton & initialization
        private Facility_Data() { }
        #endregion

        #region getters & setters
        //test
        public List<Tuple<int, int>> GetCordinate(string id)
        {
            //todo
            return new List<Tuple<int, int>>();
        }

        public Dictionary<string, string> GetFacility_Structure(string id)
        {
            return Table[id];
        }
        #endregion

        #region public methods
        public static async Task<Facility_Data> CreateAsync()
        {
            var parser = await CreateAsync("Facility_Data");
            var facilityData = new Facility_Data();
            facilityData.Table = parser.Table;
            return facilityData;
        }

        #endregion

        #region private methods
        #endregion
        #region Unity event methods
        #endregion
    }
}
