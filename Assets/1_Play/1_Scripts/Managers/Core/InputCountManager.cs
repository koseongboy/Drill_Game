using System;
using System.Collections.Generic;
using DrillGame._1_Play._1_Scripts.Components;
using DrillGame.Core.Managers;
using UnityEngine;

namespace DrillGame
{
    public class InputCountManager : Singleton_CSharp<InputCountManager>
    {
        #region Fields & Properties
        private static InputCountManager instance;
        
        private int inputCount;
        private int tickCount;

        public event Action<int> OnInputCountChanged;
        public event Action<int> OnTickCountChanged;
        
        #endregion
        
        #region Singleton & initialization
        protected override void Init()
        {
            inputCount = 0;
            tickCount = 0;
        }
        
        #endregion
        
        
        #region getters & setters

        public void addInputCount()
        {
            inputCount++;
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
