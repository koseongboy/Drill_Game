using DrillGame.Core.Facility;
using UnityEngine;

namespace DrillGame
{
    public class UI_LabDetailPopup : UITemplate_DetailPopup
    {
        #region Fields & Properties
        private FacilityEntity labFacilityEntity;
        #endregion
        
        #region getters & setters
        public override void SetData(object entity)
        {
            labFacilityEntity = (FacilityEntity)entity;
            UpdateDetail();
        }
        #endregion

        #region public methods
        public override void MoveOnBoard()
        {
            Debug.Log("MoveEngineOnBoard 진입.");
            
            CloseUI();
            
            // 이거 어디있지
            // labFacilityEntity.MoveEntity(); 
        }
        
        public override void DeleteOnBoard()
        {
            // TODO : 진짜로 철거할 거냐고 물어보기 (Confirm UI)
            // Debug.Log("DeleteEngineOnBoard 진입.");
            
            CloseUI();
            labFacilityEntity.DeleteEntity();
        }
        #endregion

        #region private methods
        protected override void UpdateDetail()
        {
            var id = labFacilityEntity.GetFacilityId();
            var data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(id);

            titleTxt.text = data.DisplayName;
            descTxt.text = data.Desc;
            // TODO : 파일명으로 이미지 불러오는 로직
        }
        #endregion
    }
}
