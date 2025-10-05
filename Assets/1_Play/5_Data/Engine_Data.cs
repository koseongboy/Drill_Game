using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System;

namespace DrillGame.Data
{
    public struct Engine_Structure
    {
        public string id;
        public string type;
        public int level;
        public List<Tuple<int, int>> coordinates;
    }

    public class Engine_Data
    {
        #region Fields & Properties
        private Dictionary<string, Engine_Structure> engineTable = new Dictionary<string, Engine_Structure>();
        int numCol = 4;
        #endregion

        #region Singleton & initialization
        private Engine_Data() { }
        #endregion

        #region getters & setters
        public List<Tuple<int, int>> GetCoordinate(string id)
        {
            string type = id.Split('-')[0];
            int level = int.Parse(id.Split('-')[1]);
            List<Tuple<int, int>> coordinates = new List<Tuple<int, int>>();
            for (int i = 1; i <= level; i++)
            {
                string stringForSearch = $"{type}-{i}";
                if (engineTable.TryGetValue(stringForSearch, out Engine_Structure structure))
                {
                    coordinates.AddRange(structure.coordinates);
                }
                else
                {
                    Debug.LogError($"Engine ID {stringForSearch} not found in the data.");
                }
            }
            return coordinates;
        }
        #endregion

        #region public methods
        public static async Task<Engine_Data> CreateAsync()
        {
            var parser = new Engine_Data();
            TextAsset csvData = await Addressables.LoadAssetAsync<TextAsset>("Engine_Data").Task;
            if (csvData != null)
            {
                parser.Parse(csvData.text);
                Addressables.Release(csvData);
                Debug.Log("Engine_Data CSV loaded and parsed successfully.");
            }
            else
            {
                Debug.LogError("Failed to load Engine_Data CSV.");
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
                    Engine_Structure structure = new Engine_Structure
                    {
                        id = fields[0],
                        type = fields[1],
                        level = int.Parse(fields[2]),
                    };
                    structure.coordinates = new List<Tuple<int, int>>();
                    string[] coord = fields[3].Split(';');
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
                    engineTable[structure.id] = structure;

                }
                else
                {
                    Debug.LogWarning($"Line {i + 1} is malformed: {line}");
                }
            }
            #endregion

            #region Unity event methods
            #endregion
        }
    }
}
