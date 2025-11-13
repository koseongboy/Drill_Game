using System.Threading.Tasks;
using UnityEngine;

namespace DrillGame.Data
{
    public class Research_Data: CSV_Data
    {
        #region Fields & Properties
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public static async Task<Research_Data> CreateAsync()
        {
            var parser = await CreateAsync("Research_Data");
            var researchdata = new Research_Data();
            researchdata.Table = parser.Table;
            return researchdata;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}