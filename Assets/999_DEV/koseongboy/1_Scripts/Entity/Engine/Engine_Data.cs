using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System;

namespace DrillGame.Entity.Engine
{
    public struct Engine_Structure
    {
        public int id;
        public int type;
        public int level;
        public int relative_x;
        public int relative_y;
    }

    public class Engine_Data
    {
        #region Fields & Properties
        private Dictionary<int, Engine_Structure> engineTable = new Dictionary<int, Engine_Structure>();
        #endregion

        #region Singleton & initialization
        private Engine_Data() { }
        #endregion

        #region getters & setters
        public Tuple<int, int>[] GetCoordinate(int id)
        {
            Tuple<int, int>[] coordinates = new Tuple<int, int>[id % 10];
            for (int i = 1; i <= id % 10; i++)
            {
                if (engineTable.TryGetValue(id / 10 * 10 + i, out Engine_Structure structure))
                {
                    coordinates[i - 1] = new Tuple<int, int>(structure.relative_x, structure.relative_y);
                }
                else
                {
                    Debug.LogWarning($"Engine ID {id + i} not found in the data.");
                    coordinates[i - 1] = new Tuple<int, int>(0, 0); // Default value if not found
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
            string[] lines = csvText.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i <lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(',');
                if (fields.Length >= 5)
                {
                    Engine_Structure structure = new Engine_Structure
                    {
                        id = int.Parse(fields[0]),
                        type = int.Parse(fields[1]),
                        level = int.Parse(fields[2]),
                        relative_x = int.Parse(fields[3]),
                        relative_y = int.Parse(fields[4])
                    };
                    engineTable[structure.id] = structure;
                }
            }
        }
        #endregion

        #region Unity event methods
        #endregion
    }
}
