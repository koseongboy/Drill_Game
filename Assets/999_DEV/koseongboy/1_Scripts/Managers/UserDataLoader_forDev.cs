using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrillGame.Managers;

namespace DrillGame
{
    public class UserDataLoader_forDev : MonoBehaviour
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
            Debug.Log("임시 유저 데이터 입력중..");
            DataLoadManager.Instance.SetUserData(new Dictionary<string, List<string>>()
            {
                { "Engine", new List<string> { "normal-2", "special-1" } },
                { "Facility", new List<string> { "iron-1", "gold-1" } },
                { "Ground", new List<string> { "150", "5"} } //depth, hp
            });
            Debug.Log("임시 유저 데이터 입력 완료!");
        }

        private void Update()
        {
        
        }
        #endregion
    }
}