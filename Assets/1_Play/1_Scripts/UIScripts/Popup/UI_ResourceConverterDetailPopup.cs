using DrillGame.Core.Facility;
using DrillGame.Core.Managers;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;

namespace DrillGame
{
    public class UI_ResourceConverterDetailPopup : UITemplate_DetailPopup
    {
        #region Fields & Properties
        private ResourceConverterEntity rcEntity;
        #endregion

        #region Singleton & initialization
        
        #endregion

        #region getters & setters
        public override void SetData(object entity)
        {
            rcEntity = (ResourceConverterEntity)entity;
            if (rcEntity.synergyCount > 0)
            {
                SetIsOnSynergy(true);
            }
            else
            {
                SetIsOnSynergy(false);
            }
            
            var data = ScriptableObjectManager.Instance.GetData<Facility_Data_>(rcEntity.GetFacilityId());
            UpdateDetail(data.DisplayName, data.GetFacilityDesc()+ "\n"+rcEntity.synergyText, data.Icon);
        }
        #endregion

        #region public methods
        public override void MoveOnBoard()
        {
            Debug.Log("MoveEngineOnBoard 진입.");
            CloseUI();
            rcEntity.MoveEntity(); 
        }
        
        public override void DeleteOnBoard()
        {
            // TODO : 진짜로 철거할 거냐고 물어보기 (Confirm UI)
            // Debug.Log("DeleteEngineOnBoard 진입.");
            
            CloseUI();
            rcEntity.DeleteEntity();
        }
 
        public void OpenResourceConverterUI()
        {
            UILoader.Instance.ShowUI("UI_ResourceConverter");
        }

        public void TryUpgradeEngineMergerLevel()
        { 
            // 1. 레벨업 가능한지 체크
            var coreLevel = CoreManager.Instance.GetCoreLevel();
            var targetEMLevel = rcEntity.data.Level + 1;

            var upgradeItemId = rcEntity.data.BuildResourceId;
            var upgradeItemCount = rcEntity.data.BuildResourceCount;
            
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
            rcEntity.UpgradeLevel();
            UILoader.Instance.ShowAlert($"자원 변환기가 업그레이드 되었습니다.");
            CloseUI();
        }
        #endregion

        #region private methods
        #endregion
        
    }
}