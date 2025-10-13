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
            var engineTable = DataLoadManager.Instance.EngineData.Table;
            var facilityTable = DataLoadManager.Instance.FacilityData.Table;
            var groundTable = DataLoadManager.Instance.GroundData.Table;
            var userData = DataLoadManager.Instance.UserData;
            var examCoordinate = DataLoadManager.Instance.EngineData.GetCoordinate("special-2");
            Debug.Log($"Engine Table Count: {engineTable.Count}");
            Debug.Log($"Facility Table Count: {facilityTable.Count}");
            Debug.Log($"Ground Table Count: {groundTable.Count}");
            Debug.Log($"User Data Count: {userData.Count}");
            Debug.Log($"Exam Coordinate Count: {examCoordinate.Count}");

        }

        private void Update()
        {
        
        }
        #endregion
    }
}