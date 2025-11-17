using System.Collections.Generic;
using UnityEngine;

namespace DrillGame
{
    public class ScriptableObjectManager : MonoBehaviour
    {
        #region Fields & Properties
        #endregion
    
        #region Singleton & initialization 
        public static ScriptableObjectManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            
            LoadData();
        }
        #endregion
    
        #region getters & setters

        #endregion
    
        #region public methods
        #endregion
    
        #region private methods

        private void LoadData()
        {

        }
        #endregion
    }
}
