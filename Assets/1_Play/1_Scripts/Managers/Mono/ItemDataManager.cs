using System.Collections.Generic;
using UnityEngine;

namespace DrillGame
{
    public class ItemDataManager : MonoBehaviour
    {
        #region Fields & Properties
        private Dictionary<string, ItemData> itemDataDict = new Dictionary<string, ItemData>();
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
        public ItemData GetItemData(string itemId)
        {
            if (itemDataDict.TryGetValue(itemId, out ItemData itemData))
            {
                return itemData;
            }
            Debug.LogWarning($"경고: 올바르지 않은 ItemId입니다. (string 형식이 맞는지도 확인하세요.) : {itemId}");
            return null;
        }
        #endregion
    
        #region public methods
        public void PrintItemData_Test(string id)
        {
            var itemData = GetItemData(id);
            Debug.Log(itemData.ToString());
        }
        #endregion
    
        #region private methods

        private void LoadItemDictionary()
        {
            itemDataDict.Clear();
            ItemData[] allResources = Resources.LoadAll<ItemData>("ScriptableObject/ItemData");
        
            foreach (var resource in allResources)
            {
                if (itemDataDict.ContainsKey(resource.itemId))
                {
                    Debug.LogError($"[ERROR] 자원 ID가 중복되었습니다: {resource.itemId} : {resource.displayName}");
                    continue;
                }
            
                itemDataDict.Add(resource.itemId, resource);
            }
        }
        #endregion
    }
}
