using DrillGame.Core.Facility;
using DrillGame.UI;
using UnityEngine;

namespace DrillGame
{
    public class UI_EngineMergerDetailPopup : UITemplate_DetailPopup
    {
        #region Fields & Properties
        private EngineMergerEntity engineMergerFacilityEntity;
        #endregion
        
        #region getters & setters
        public override void SetData(object entity)
        {
            engineMergerFacilityEntity = (EngineMergerEntity)entity;
            UpdateDetail();
        }
        #endregion

        #region public methods
        public override void MoveOnBoard()
        {
            Debug.Log("MoveEngineOnBoard 진입.");
            
            CloseUI();
            
            engineMergerFacilityEntity.MoveEntity(); 
        }
        
        public override void DeleteOnBoard()
        {
            // TODO : 진짜로 철거할 거냐고 물어보기 (Confirm UI)
            // Debug.Log("DeleteEngineOnBoard 진입.");
            
            CloseUI();
            engineMergerFacilityEntity.DeleteEntity();
        }
 
        public void OpenEngineMergerUI()
        {
            UILoader.Instance.ShowUI("UI_EngineMerger");
        }

        public void TryUpgradeEngineMergerLevel()
        {
            UILoader.Instance.ShowAlert("자원이 부족합니다.\n필요 자원 : 철 자재 4000개");
            
            // // 1. 레벨업 가능한지 체크
            // var coreLevel = CoreManager.Instance.GetCoreLevel();
            // var targetEMLevel = engineMergerFacilityEntity.data.Level + 1;
            //
            // if (coreLevel < targetEMLevel)
            // {
            //     // 불가능
            //     return;
            // }
            // 2. FacilityComponent의 id값을 변경하기.
            // engineMergerFacilityEntity.UpgradeLevel();
        }
        #endregion

        #region private methods
        protected override void UpdateDetail()
        {
            var id = engineMergerFacilityEntity.GetFacilityId();
            var data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(id);

            titleTxt.text = data.DisplayName;
            descTxt.text = data.GetFacilityDesc();
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
