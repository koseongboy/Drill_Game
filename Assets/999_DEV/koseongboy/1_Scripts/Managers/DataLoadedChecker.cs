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
        IEnumerator DataCheckCoroutine()
        {
            yield return new WaitForSeconds(2.0f); //1초 대기
            var engineTable = DataLoadManager.Instance.GetEngineTable();
            var facilityTable = DataLoadManager.Instance.GetFacilityTable();
            var groundTable = DataLoadManager.Instance.GetGroundTable();
            var userData = DataLoadManager.Instance.GetUserData();
            Debug.Log($"Engine Table Count: {engineTable.Count}");
            Debug.Log($"Facility Table Count: {facilityTable.Count}");
            Debug.Log($"Ground Table Count: {groundTable.Count}");
            Debug.Log($"User Data Count: {userData.Count}");
        }
        #endregion

        #region Unity event methods
        private void Awake()
        {

        }

        private void Start()
        {
            StartCoroutine(DataCheckCoroutine());
        }

        private void Update()
        {
        
        }
        #endregion
    }
}