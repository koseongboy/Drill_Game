using System.Collections.Generic;
using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Managers;
using DrillGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

using DrillGame.UI.Interface;
using TMPro;

namespace DrillGame
{
    public class UI_EngineDetailPopup : UITemplate_DetailPopup
    {
        #region Fields & Properties
        private EngineEntity engineEntity;
        #endregion

        #region getters & setters

        public override void SetData(object entity)
        {
            engineEntity = (EngineEntity)entity;
            var engineData = ScriptableObjectManager.Instance.GetData<Engine_Data_>(engineEntity.GetEngineId());
            
            UpdateDetail(engineData.DisplayName, engineData.Desc, engineData.Icon);
        }
        #endregion

        #region public methods
        public override void MoveOnBoard()
        {
            Debug.Log("MoveEngineOnBoard 진입.");
            
            CloseUI();
            engineEntity.MoveEntity();
        }
        
        public override void DeleteOnBoard()
        {
            // TODO : 진짜로 철거할 거냐고 물어보기 (Confirm UI)
            // Debug.Log("DeleteEngineOnBoard 진입.");
            
            CloseUI();
            engineEntity.DeleteEntity();
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion

    }
}
