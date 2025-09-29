using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DrillGame.Entity.Engine
{
    public class Engine_Core
    {
        #region Fields & Properties
        public Vector2Int position { get; private set; }
        public ulong totalTickCount { get; private set; }

        private readonly List<Engine_Base> _engines = new List<Engine_Base>();
        public IReadOnlyList<Engine_Base> Engines => _engines;
        #endregion

        #region Singleton & initialization
        private static Engine_Core _instance;
        public static Engine_Core Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Engine_Core();
                }
                return _instance;
            }
        }
        private Engine_Core()
        {
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("Engine_Core initialized.");
            // Initialization logic here
        }

        #endregion

        #region getters & setters
        public Vector2Int Position
        {
            get => position;
            set => position = value;
        }
        #endregion

        #region public methods
        public void AddEngine(Engine_Base engine)
        {
            if (engine == null)
            {
                Debug.LogError("Cannot add a null engine.");
                return;
            }
            if (_engines.Contains(engine))
            {
                Debug.LogWarning("Engine already exists in the core.");
                return;
            }
            _engines.Add(engine);
        }

        public void RemoveEngine(Engine_Base engine)
        {
            if (engine == null)
            {
                Debug.LogError("Cannot remove a null engine.");
                return;
            }
            if (!_engines.Contains(engine))
            {
                Debug.LogWarning("Engine not found in the core.");
                return;
            }
            _engines.Remove(engine);
        }

        public void ActivateCore()
        {
            ActivateAllEngines();
        }

        public void ActivateEngine(int index)
        {
            ActivateEngineByIndex(index);
        }

        public void AddTickCount()
        {
            totalTickCount++;
        }

        #endregion

        #region private methods
        private void ActivateAllEngines()
        {
            foreach (var engine in _engines)
            {
                engine.Activate();
            }
        }

        private void ActivateEngineByIndex(int index)
        {
            if (index < 0 || index >= _engines.Count)
            {
                Debug.LogError("Index out of range.");
                return;
            }
            _engines[index].Activate();
        }


        #endregion
    }

}
