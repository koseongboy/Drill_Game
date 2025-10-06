using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrillGame.Managers;

namespace DrillGame
{
    public class DataLoadedChecker : MonoBehaviour
    {
        #region Fields & Properties
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        #endregion

        #region private methods

            

        #endregion

        #region Unity event methods
        private void Awake()
        {

        }

        private void Start()
        {
            var engineTable = DataLoadManager.Instance.EngineTable;
            var facilityTable = DataLoadManager.Instance.FacilityTable;
            var groundTable = DataLoadManager.Instance.GroundTable;
            var userData = DataLoadManager.Instance.UserData;
            Debug.Log($"Engine Table Count: {engineTable.Count}");
            Debug.Log($"Facility Table Count: {facilityTable.Count}");
            Debug.Log($"Ground Table Count: {groundTable.Count}");
            Debug.Log($"User Data Count: {userData.Count}");
        }

        private void Update()
        {
        
        }
        #endregion
    }
}