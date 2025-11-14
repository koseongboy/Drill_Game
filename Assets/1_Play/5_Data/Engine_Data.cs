using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace DrillGame.Data
{
    public class Engine_Data: CSV_Data
    {
        #region Fields & Properties
        #endregion

        #region Singleton & initialization
        private Engine_Data() { }
        #endregion

        #region getters & setters
        
        public List<Tuple<int, int>> GetCoordinate(string id)
        {
            var coordinates = new List<Tuple<int, int>>();
            string type = id.Split('-')[0];
            int level = int.Parse(id.Split('-')[1]);

            for (int i = 1; i <= level; i++)
            {
                string stringForSearch = $"{type}-{i}";
                if (Table.TryGetValue(stringForSearch, out var engineData))
                {
                    // "coordinates"라는 키로 좌표 문자열을 가져옵니다.
                    if (engineData.TryGetValue("Coordinates", out string coordString))
                    {
                        var coordPairs = coordString.Split(';');
                        foreach (var pair in coordPairs)
                        {
                            var xy = pair.Split(',');
                            if (xy.Length == 2 && int.TryParse(xy[0], out int x) && int.TryParse(xy[1], out int y))
                            {
                                coordinates.Add(new Tuple<int, int>(x, y));
                            }
                        }
                    }
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
            var parser = await CreateAsync("Engine_Data");
            var engineData = new Engine_Data();
            engineData.Table = parser.Table;
            return engineData;
        }
        #endregion

        #region private methods
        #endregion
        #region Unity event methods
        #endregion
    }
}
