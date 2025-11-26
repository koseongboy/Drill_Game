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
            UpdateDetail();
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
        protected override void UpdateDetail()
        {
            var id = engineEntity.GetEngineId(); 
            var data = ScriptableObjectManager.Instance.GetData<Engine_Data_>(id);

            titleTxt.text = data.DisplayName;
            descTxt.text = data.Desc;
            
            // Sprite
            Sprite icon = Resources.Load<Sprite>("Icon/ItemIcon/" + data.Icon);
            if (icon == null)
            {
                Debug.LogError($"Error: Resources 폴더에서 스프라이트 자원을 찾을 수 없습니다. : {data.Icon}");
            }
            iconImg.sprite = icon;
        }
        #endregion

        #region Unity event methods
        #endregion

    }
}
