using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrillGame.Data
{
    public class Ground_Data: CSV_Data
    {
        #region Fields & Properties
        public List<int> DepthRanges {get; set;} = new List<int>();
        public new Dictionary<int, Dictionary<string, string>> Table { get; protected set; } = new Dictionary<int, Dictionary<string, string>>();
        #endregion

        #region Singleton & initialization
        private Ground_Data() { }

        #endregion

        #region getters & setters
        
        #endregion

        #region public methods
        public static async Task<Ground_Data> CreateAsync()
        {
            var parser = await CreateAsync("Ground_Data");
            var groundData = new Ground_Data();
            foreach (var entry in parser.Table.Keys)
            {
                groundData.DepthRanges.Add(int.Parse(entry));
            }
            foreach (var key in parser.Table.Keys) { groundData.Table[int.Parse(key)] = parser.Table[key]; }
            return groundData;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion

    }
}
