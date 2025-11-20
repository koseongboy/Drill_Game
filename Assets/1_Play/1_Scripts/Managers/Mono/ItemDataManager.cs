using System.Collections.Generic;
using DrillGame.Data;
using UnityEngine;

namespace DrillGame
{
    public class ItemDataManager : MonoBehaviour
    {
        #region Fields & Properties
        private Dictionary<int, Item_Data_> itemDataDict = new Dictionary<int, Item_Data_>();
        #endregion
    
        #region Singleton & initialization 
        public static ItemDataManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
            
            LoadItemDictionary();
        }
        #endregion
    
        #region getters & setters
        public Item_Data_ GetItemData(int itemId)
        {
            if (itemDataDict.TryGetValue(itemId, out Item_Data_ itemData))
            {
                return itemData;
            }
            Debug.LogWarning($"경고: 올바르지 않은 ItemId입니다. (string 형식이 맞는지도 확인하세요.) : {itemId}");
            return null;
        }
        #endregion
    
        #region public methods
        public void PrintItemData_Test(int id)
        {
            var itemData = GetItemData(id);
            Debug.Log(itemData.ToString());
        }
        #endregion
    
        #region private methods

        private void LoadItemDictionary()
        {
            itemDataDict.Clear();
            Item_Data_[] allResources = Resources.LoadAll<Item_Data_>("ScriptableObject/ItemData/Resource");
        
            foreach (var resource in allResources)
            {
                if (itemDataDict.ContainsKey(resource.Id))
                {
                    Debug.LogError($"[ERROR] 자원 ID가 중복되었습니다: {resource.Id} : {resource.DisplayName}");
                    continue;
                }
            
                itemDataDict.Add(resource.Id, resource);
            }
        }
        #endregion
    }
}
