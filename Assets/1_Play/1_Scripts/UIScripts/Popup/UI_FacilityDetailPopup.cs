using System.Collections.Generic;
using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.Core.Facility;
using DrillGame.Managers;
using DrillGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

using DrillGame.UI.Interface;
using TMPro;

namespace DrillGame
{
    public class UI_FacilityDetailPopup : UITemplate_DetailPopup
    {
        #region Fields & Properties
        private FacilityEntity facilityEntity;
        #endregion

        #region getters & setters

        public override void SetData(object entity)
        {
            facilityEntity = (FacilityEntity)entity;
            if (facilityEntity.synergyCount > 0)
            {
                SetIsOnSynergy(true);
            }
            else
            {
                SetIsOnSynergy(false);
            }
            
            var id = facilityEntity.GetFacilityId(); 
            if (id == 0)
            {
                Debug.LogError("Facility에 지정된 Facility Id가 없습니다. UI를 업데이트할 수 없습니다.");
                return;
            }
            var data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(id);

            UpdateDetail(data.DisplayName, data.GetFacilityDesc() + "\n"+facilityEntity.synergyText, data.Icon);
        }
        #endregion

        #region public methods
        public override void MoveOnBoard()
        {
            CloseUI();
            facilityEntity.MoveEntity();
        }
        
        public override void DeleteOnBoard()
        {
            // TODO : 진짜로 철거할 거냐고 물어보기 (Confirm UI)
            // Debug.Log("DeleteEngineOnBoard 진입.");
            
            CloseUI();
            facilityEntity.DeleteEntity();
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}
