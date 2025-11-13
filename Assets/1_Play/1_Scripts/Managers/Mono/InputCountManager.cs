using System.Collections.Generic;
using UnityEngine;

namespace DrillGame
{
    public class InputCountManager : MonoBehaviour
    {
        #region Fields & Properties
        private int inputCount = 0;
        private int tickCount = 0;
        private List<IInputCountObserver> observers = new List<IInputCountObserver>();
        #endregion

        #region Singleton & initialization
        public static InputCountManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
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
