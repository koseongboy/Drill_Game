using DrillGame.Entity.Engine;
using DrillGame.Entity.Facility;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Managers
{
    public class BoardManager : MonoBehaviour
    {
        #region Fields & Properties
        private Dictionary<Vector2Int, Engine_Base> enginePositions = new();
        private Dictionary<Vector2Int, Facility_Base> facilityPositions = new();

        #endregion

        #region Singleton & initialization
        public static BoardManager Instance { get; private set; }


        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void EngineAction(List<Vector2Int> targetPositions, string name)
        {
            Debug.Log($"EngineAction called by {name} for positions: ");
        }
        #endregion

        #region public methods - registration
        public void RegisterEngine(Engine_Base engine)
        {

        }

        public void RegisterFacility(Facility_Base facility)
        {
            
        }

        public void UnregisterEngine(Engine_Base engine)
        {

        }
        public void UnregisterFacility(Facility_Base facility)
        {

        }


        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }
        #endregion
    }
}
