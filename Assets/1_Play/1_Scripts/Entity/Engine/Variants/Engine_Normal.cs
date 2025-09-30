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
        public Engine_Normal(EngineController engineController, Vector2Int position) : base(engineController, position)
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
            base.ActivateEngine();
            // 개별 처리 로직 - 필요시
        }

        
        #endregion

        #region Unity event methods
        #endregion

    }
}
