using System;
using System.Collections.Generic;
using DrillGame.Core.Managers;
using DrillGame.Managers;
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
            LoadInputCount();
            SaveManager.OnRequestAllDataSave += SaveInputCount;
        }
        
        #endregion
        
        
        #region getters & setters

        public void AddCoreActiveCount()
        {
            coreActiveCount++;
            if (coreActiveCount >= 8)
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
        private void SaveInputCount()
        {
            SaveManager.Instance.SaveInputCountData(inputCount);
        }
        
        private void LoadInputCount()
        {
            var savedTodayInputCount = SaveManager.Instance.LoadTodayInputCount();
            inputCount = savedTodayInputCount;
            Debug.Log(inputCount);
            OnInputCountChanged?.Invoke(inputCount);
        }
        #endregion
        
        #region DEV
        public void SetInputCount()
        {
            inputCount = 5;
        }
        #endregion
    }
}
