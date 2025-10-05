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
        public string id;
        public string name;
        public int level;
        public List<Tuple<int, int>> coordinates;
    }
    public class Facility_Data
    {
        #region Fields & Properties
        private Dictionary<string, Facility_Structure> facilityTable = new Dictionary<string, Facility_Structure>();
        // column의 개수. 꼭 바꿀 것
        private int numCol = 4;
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

        public Facility_Structure GetFacility_Structure(string id)
        {
            return facilityTable[id];
        }
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
                Debug.Log("Facility_Data CSV loaded and parsed successfully.");
            }
            else
            {
                Debug.LogError("Failed to load Facility_Data CSV.");
            }

            return parser;
        }

        #endregion

        #region private methods
        private void Parse(string csvText)
        {
            string[] lines = csvText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split('|');
                if (fields.Length == numCol)
                {
                    Facility_Structure structure = new Facility_Structure
                    {
                        id = fields[0],
                        name = fields[1],
                        level = int.Parse(fields[2]),
                    };
                    string[] coord = fields[3].Split(';');
                    structure.coordinates = new List<Tuple<int, int>>();
                    for (int k = 0; k < coord.Length; k++)
                    {
                        string[] xy = coord[k].Split(',');
                        if (xy.Length == 2)
                        {
                            if (int.TryParse(xy[0], out int x) && int.TryParse(xy[1], out int y))
                            {
                                structure.coordinates.Add(new Tuple<int, int>(x, y));
                            }
                            else
                            {
                                Debug.LogWarning($"좌표가 이상해요~ at line {i + 1}.");
                            }
                        }
                    }



                    facilityTable[structure.id] = structure;
                }
                else
                {
                    Debug.LogWarning($"numCol 수정했나요??");
                }
            }


        }
        #endregion
        #region Unity event methods
        #endregion
    }
}
