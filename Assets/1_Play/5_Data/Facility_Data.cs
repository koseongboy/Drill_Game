using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal.Internal;
using System;


namespace DrillGame.Data
{
    public struct Facility_Structure
    {
        public int id;
        public string name;
        public int length;
        public int height;
    }
    public class Facility_Data
    {
        #region Fields & Properties
        private Dictionary<int, Facility_Structure> facilityTable = new Dictionary<int, Facility_Structure>();
        // column의 개수. 꼭 바꿀 것
        private int numCol = 4;
        #endregion

        #region Singleton & initialization
        private Facility_Data() { }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public static async Task<Facility_Data> CreateAsync()
        {
            var parser = new Facility_Data();
            TextAsset csvData = await Addressables.LoadAssetAsync<TextAsset>("Facility_Data").Task;
            if (csvData != null)
            {
                parser.Parse(csvData.text);
                Addressables.Release(csvData);
            }
            else
            {
                Debug.LogError("Failed to load Engine_Data CSV.");
            }

            return parser;
        }
        //test
        public Tuple<int, int> GetSize(int id)
        {
            return new Tuple<int, int>(facilityTable[id].length, facilityTable[id].height);
        }
        #endregion

        #region private methods
        private void Parse(string csvText)
        {
            string[] lines = csvText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(',');
                if (fields.Length >= numCol)
                {
                    Facility_Structure structure = new Facility_Structure
                    {
                        id = int.Parse(fields[0]),
                        name = fields[1],
                        length = int.Parse(fields[2]),
                        height = int.Parse(fields[3]),
                    };
                    facilityTable[structure.id] = structure;
                }
            }
        }
        #endregion

        #region Unity event methods
        #endregion

    }
}
