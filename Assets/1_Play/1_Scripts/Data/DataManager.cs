using UnityEngine;

namespace DrillGame.Managers
{
    public class DataManager
    {
        #region Fields & Properties
        public static DataManager Instance { get; private set; }

        #endregion

        #region Singleton & initialization
        public DataManager()
        {
            if (Instance != null)
            {
                Debug.LogWarning("DataManager instance already exists. Destroying duplicate.");
                return;
            }
            Instance = this;
            
        }
        #endregion

        #region getters & setters

        #endregion

        #region public methods
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
