using System.Collections.Generic;
using UnityEngine;

namespace DrillGame
{
    public class InputCountManager
    {
        #region Fields & Properties
        private static InputCountManager instance;
        
        private int inputCount;
        private int tickCount;
        private List<IInputCountObserver> observers;
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
            observers = new List<IInputCountObserver>();
        }
        
        #endregion
        
        
        #region getters & setters

        public void addInputCount()
        {
            inputCount++;
            InputCountChanged();
        }

        public void addTickCount()
        {
            tickCount++;
            TickCountChanged();
        }
        #endregion

        #region public methods
        // InputCount 옵저빙
        public void AddInputCountObserver(IInputCountObserver observer)
        {
            // Debug.Log(observer + " : Observer added.");
            observers.Add(observer);
        }
        #endregion

        #region private methods

        private void InputCountChanged()
        {
            foreach (var observer in observers)
            {
                observer.OnInputCountChanged(inputCount);
            }
        }

        private void TickCountChanged()
        {
            foreach (var observer in observers)
            {
                observer.OnTickCountChanged(tickCount);
            }
        }

        #endregion

        #region Unity event methods
        #endregion
    }

    public interface IInputCountObserver
    {
        public void OnInputCountChanged(int count);
        public void OnTickCountChanged(int count);
    }
}
