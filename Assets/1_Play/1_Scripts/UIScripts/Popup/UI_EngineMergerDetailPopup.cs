using DrillGame.Core.Facility;
using DrillGame.Core.Managers;
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
            if (engineMergerFacilityEntity.synergyCount > 0)
            {
                SetIsOnSynergy(true);
            }
            else
            {
                SetIsOnSynergy(false);
            }
            
            var data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(engineMergerFacilityEntity.GetFacilityId());
            UpdateDetail(data.DisplayName, data.GetFacilityDesc() + "\n"+engineMergerFacilityEntity.synergyText, data.Icon);
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
            UILoader.Instance.ShowUI_EngineMerger( engineMergerFacilityEntity.data.Level );
        }

        public void TryUpgradeEngineMergerLevel()
        { 
            // 1. 레벨업 가능한지 체크
            var coreLevel = CoreManager.Instance.GetCoreLevel();
            var targetEMLevel = engineMergerFacilityEntity.data.Level + 1;

            var upgradeItemId = engineMergerFacilityEntity.data.BuildResourceId;
            var upgradeItemCount = engineMergerFacilityEntity.data.BuildResourceCount;
            
            if (coreLevel < targetEMLevel)
            {
                UILoader.Instance.ShowAlert($"코어 레벨이 부족합니다.\n필요 레벨 : {targetEMLevel}   현재 : {coreLevel}");
                return;
            }
            if (!InventoryManager.Instance.HasItem(upgradeItemId, upgradeItemCount))
            {
                // 불가능
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(upgradeItemId);
                UILoader.Instance.ShowAlert($"자원이 부족합니다.\n필요 자원 : {itemData.DisplayName} {upgradeItemCount}개");
                return;
            }
            
            // 2. FacilityComponent의 id값을 변경하기.
            InventoryManager.Instance.TryRemoveItem(upgradeItemId, upgradeItemCount);
            engineMergerFacilityEntity.UpgradeLevel();
            UILoader.Instance.ShowAlert($"엔진 합성기가 업그레이드 되었습니다.");
            CloseUI();
        }
        #endregion

        #region private methods
        #endregion
    }
}
