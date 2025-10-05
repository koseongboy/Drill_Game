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
            var engineTable = DataLoadManager.Instance.GetEngineTable();
            var facilityTable = DataLoadManager.Instance.GetFacilityTable();
            var groundTable = DataLoadManager.Instance.GetGroundTable();
            var userData = DataLoadManager.Instance.GetUserData();
            var groundPrefab = DataLoadManager.Instance.GetGroundPrefab();
            Debug.Log($"Engine Table Count: {engineTable.Count}");
            Debug.Log($"Facility Table Count: {facilityTable.Count}");
            Debug.Log($"Ground Table Count: {groundTable.Count}");
            Debug.Log($"User Data Count: {userData.Count}");
            Debug.Log($"Ground Prefab: {groundPrefab.name}");
        }

        private void Update()
        {
        
        }
        #endregion
    }
}