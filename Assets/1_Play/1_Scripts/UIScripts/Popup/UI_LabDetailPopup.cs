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
            Debug.Log(entity.ToString());
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
            // Sprite
            Sprite icon = Resources.Load<Sprite>("Icon/ItemIcon/" + data.Icon);
            if (icon == null)
            {
                Debug.LogError($"Error: Resources 폴더에서 스프라이트 자원을 찾을 수 없습니다. : {data.Icon}");
            }
            iconImg.sprite = icon;
        }
        #endregion
    }
}
