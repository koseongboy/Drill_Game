using DrillGame.Components.Engine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace DrillGame.Entity.Engine
{
    public class Engine_Normal : Engine_Base
    {
        #region Fields & Properties
        //string engineName;    //상위 클래스에 있음
        //float waitDelay;

        #endregion

        #region Singleton & initialization
        public Engine_Normal(EngineController engineController, Vector2 position) : base(engineController, position)
        {
            Initialize();
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        
        #endregion

        #region private methods
        private void Initialize()
        {

            
        }

        

        protected override void ActivateEngine()
        {
            Debug.Log($"{engineName} 엔진 활성화!");
            // 처리 로직
            OnActivated?.Invoke();
        }

        
        #endregion

        #region Unity event methods
        #endregion

    }
}
