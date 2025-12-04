using DrillGame._1_Play._1_Scripts.ScriptableObject;
using DrillGame.Core.Facility;
using DrillGame.Core.Managers;
using DrillGame.Managers;
using DrillGame.UI;
using UnityEngine;

namespace DrillGame
{
    public class UI_ItemDetailPopup : UITemplate_DetailPopup
    {
        #region Fields & Properties

        private int showingItemId;
        #endregion

        #region Singleton & initialization
        #endregion

        #region getters & setters
        public override void SetData(object entity)
        {
            showingItemId = (int)entity;
            SetIsOnSynergy(false);
            
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>( showingItemId );
            UpdateDetail(itemData.DisplayName, "", itemData.ItemIcon);
        }
        #endregion

        #region public methods
        public void RemoveItem()
        {
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(showingItemId);
            if (InventoryManager.Instance.TryRemoveItem(showingItemId))
            {
                if (itemData.GetItemType_Enum() == InventoryManager.ItemType.Facility)
                {
                    var facilityData = ScriptableObjectManager.Instance.GetData<Facility_Data_>(itemData.EntityId);
                    InventoryManager.Instance.AddItem(facilityData.BuildResourceId, facilityData.BuildResourceCount);
                }else if (itemData.GetItemType_Enum() == InventoryManager.ItemType.Engine)
                {
                    var engineData = ScriptableObjectManager.Instance.GetData<Engine_Data_>(itemData.EntityId);
                    InventoryManager.Instance.AddItem(engineData.ResourceItemId, engineData.ResourceItemCount);
                }
            }
            UILoader.Instance.ShowAlert("아이템이 삭제되었습니다.\n재료가 반환되었습니다.");
                
            CloseUI();
        }

        public void StartBatch()
        {
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>( showingItemId );
            if (itemData.GetItemType_Enum() != InventoryManager.ItemType.Facility
                && itemData.GetItemType_Enum() != InventoryManager.ItemType.Engine)
            {
                UILoader.Instance.ShowAlert("배치할 수 없습니다.\n시설이나 엔진이 아닙니다.");
                return;
            }

            GameManager.Instance.BatchEntity(showingItemId);
            CloseUI();
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion
    }
}