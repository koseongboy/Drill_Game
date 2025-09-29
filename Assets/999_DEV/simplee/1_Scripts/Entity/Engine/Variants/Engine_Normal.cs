using UnityEngine;

namespace DrillGame.Entity.Engine
{
    public class Engine_Normal : Engine_Base
    {
        #region Fields & Properties
        private string engineName;

        #endregion

        #region Singleton & initialization
        public Engine_Normal()
        {
            Initialize();
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public override void Activate()
        {
            Debug.Log($"{engineName} 활성화됨.");
            OnActivated?.Invoke();
        }
        #endregion

        #region private methods
        private void Initialize()
        {
            engineName = GetType().Name;
            Engine_Core.Instance.AddEngine(this);

            Debug.Log($"{engineName} 생성 및 코어 register.");
        }
        #endregion

        #region Unity event methods
        #endregion

    }
}
