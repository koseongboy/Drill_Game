using DrillGame.Entity.Engine;
using DrillGame.Entity.Facility;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Managers
{
    public class FlowManager : MonoBehaviour
    {
        #region Fields & Properties
        List<Engine_Base> engines = new List<Engine_Base>();
        List<Facility_Base> facilities = new List<Facility_Base>();

        #endregion

        #region Singleton & initialization
        public static FlowManager Instance { get; private set; }


        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void EngineAction(List<Vector2Int> targetPositions, string name)
        {
            Debug.Log($"EngineAction called for {name} at positions: {string.Join(", ", targetPositions)}");
        }
        #endregion

        #region public methods - registration
        public void RegisterEngine(Engine_Base engine)
        {
            if (engine == null)
            {
                Debug.LogError("Cannot register a null engine.");
                return;
            }
            if (engines.Contains(engine))
            {
                Debug.LogWarning("Engine already registered.");
                return;
            }
            engines.Add(engine);
            Debug.Log($"Engine registered: {engine.GetType().Name}");
        }

        public void RegisterFacility(Facility_Base facility)
        {
            if (facility == null)
            {
                Debug.LogError("Cannot register a null facility.");
                return;
            }
            if (facilities.Contains(facility))
            {
                Debug.LogWarning("Facility already registered.");
                return;
            }
            facilities.Add(facility);
            Debug.Log($"Facility registered: {facility.GetType().Name}");
        }

        public void UnregisterEngine(Engine_Base engine)
        {
            if (engine == null)
            {
                Debug.LogError("Cannot unregister a null engine.");
                return;
            }
            if (!engines.Contains(engine))
            {
                Debug.LogWarning("Engine not found in the registry.");
                return;
            }
            engines.Remove(engine);
            Debug.Log($"Engine unregistered: {engine.GetType().Name}");
        }
        public void UnregisterFacility(Facility_Base facility)
        {
            if (facility == null)
            {
                Debug.LogError("Cannot unregister a null facility.");
                return;
            }
            if (!facilities.Contains(facility))
            {
                Debug.LogWarning("Facility not found in the registry.");
                return;
            }
            facilities.Remove(facility);
            Debug.Log($"Facility unregistered: {facility.GetType().Name}");
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
