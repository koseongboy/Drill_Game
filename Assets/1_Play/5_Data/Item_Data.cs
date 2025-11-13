using System.Threading.Tasks;
using DrillGame.Data;
using UnityEngine;

namespace DrillGame.Data
{
    public class Item_Data: CSV_Data
    {
        #region Fields & Properties
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public static async Task<Item_Data> CreateAsync()
        {
            var parser = await CreateAsync("Item_Data");
            var itemdata = new Item_Data();
            itemdata.Table = parser.Table;
            return itemdata;
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}