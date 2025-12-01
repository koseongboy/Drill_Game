using System;
using System.Collections.Generic;
using DrillGame.Core.Managers;
using UnityEngine;

namespace DrillGame
{
    public class InputCountManager
    {
        #region Fields & Properties
        private static InputCountManager instance;

        private int coreActiveCount;
        private int inputCount;
        private int tickCount;

        public event Action<int> OnInputCountChanged;
        public event Action<int> OnTickCountChanged;
        
        #endregion
        
        #region Singleton & initialization
        public static InputCountManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputCountManager();
                }
                return instance;
            }
        }

        private InputCountManager()
        {
            inputCount = 0;
            tickCount = 0;
        }
        
        #endregion
        
        
        #region getters & setters

        public void AddCoreActiveCount()
        {
            coreActiveCount++;
            if (coreActiveCount >= 3)
            {
                coreActiveCount = 0;
                BoardManager.Instance.ActivateCore();
            }
            // Debug.Log("Core Active Count: " + coreActiveCount);
        }

        public void addInputCount()
        {
            inputCount++;
            AddCoreActiveCount();
            OnInputCountChanged?.Invoke(inputCount);
        }

        public void addTickCount()
        {
            tickCount++;
            OnTickCountChanged?.Invoke(tickCount);
        }
        #endregion

        #region public methods
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
