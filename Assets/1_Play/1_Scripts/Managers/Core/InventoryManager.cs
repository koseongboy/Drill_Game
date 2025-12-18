using DrillGame.Managers;
using NUnit.Compatibility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;


namespace DrillGame.Core.Managers
{
    public class InventoryManager
    {
        public const int resourceKeyStart = 1001;
        public const int resourceKeyEnd = 1099;
        public const int engineKeyStart = 1101;
        public const int engineKeyEnd = 1199;
        public const int facilityKeyStart = 1201;
        public const int facilityKeyEnd = 1500;


        public enum ItemType
        {
            Resource,
            Facility,
            Engine,
            None
        }
        
        #region Fields & Properties

        private Dictionary<int, int> inventoryItems = new();

        public Action OnInventoryUpdated;

        #endregion

        #region Singleton & initialization

        private static InventoryManager instance;
        public static InventoryManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Creating new InventoryManager instance.");
                    instance = new InventoryManager();
                    Initialize();
                }
                return instance;
            }
        }

        private static void Initialize()
        {
            instance.inventoryItems = SaveManager.Instance.LoadInventoryData(new Dictionary<int, int>());
            Debug.Log("InventoryManager initialized with " + instance.inventoryItems.Count + " items.");

            //instance.OnInventoryUpdated?.Invoke();

            SaveManager.OnRequestAllDataSave += instance.SaveInventoryData;
        }

        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void CallInventoryManager()
        {
            Debug.Log("InventoryManager called. Lazy initialization ");
            return;
        }

        public void AddItem(Item_Data_ item, int count = 1)
        {
            AddItem(item.GetId(), count);
        }

        public void AddItem(int itemId, int count = 1)
        {
            Debug.Log("Adding item : " + itemId + " : " + count);
            if (inventoryItems.ContainsKey(itemId))
            {
                inventoryItems[itemId] += count;
            }
            else
            {
                inventoryItems[itemId] = count;
            }
            // 변경 사항 알림
            OnInventoryUpdated?.Invoke();
        }

        public bool TryRemoveItem(int itemId, int count = 1)
        {
            // 연구 진척에 따른 감산 로직
            // 구리
            if (itemId == 1004 && ResearchManager.Instance.IsResearchDone(30004)) 
            {
                count = Mathf.CeilToInt(count * 0.7f); // 30% 감소
            }
            // 철
            else if (itemId == 1006 && ResearchManager.Instance.IsResearchDone(30009))
            {
                count = Mathf.CeilToInt(count * 0.7f); // 30% 감소
            }
            // 금
            else if (itemId == 1008 && ResearchManager.Instance.IsResearchDone(30014))
            {
                count = Mathf.CeilToInt(count * 0.7f); // 30% 감소
            }
            // 흑요석
            else if (itemId == 1010 && ResearchManager.Instance.IsResearchDone(30019))
            {
                count = Mathf.CeilToInt(count * 0.7f); // 30% 감소
            }
            // 텅스텐
            else if (itemId == 1012 && ResearchManager.Instance.IsResearchDone(30024))
            {
                count = Mathf.CeilToInt(count * 0.7f); // 30% 감소
            }
            // 티타늄
            else if (itemId == 1014 && ResearchManager.Instance.IsResearchDone(30029))
            {
                count = Mathf.CeilToInt(count * 0.7f); // 30% 감소
            }


            // 실제 아이템 제거 로직
            if (inventoryItems.TryGetValue(itemId, out int currentCount) && currentCount >= count)
            {
                inventoryItems[itemId] = currentCount - count;
                if (inventoryItems[itemId] == 0)
                {
                    inventoryItems.Remove(itemId);
                }
                // 변경 사항 알림
                OnInventoryUpdated?.Invoke();

                return true;
            }
            else
            {
                Debug.LogWarning($"Not enough items to remove. Item ID: {itemId}, Requested: {count}, Available: {currentCount}");
                return false;
            }
        }

        public bool TryRemoveItem(Item_Data_ item, int count = 1)
        {
            int itemId = item.GetId();
            return TryRemoveItem(itemId, count);
        }
        public void RemoveItemAll(int itemId)
        {
            if (inventoryItems.ContainsKey(itemId))
            {
                inventoryItems.Remove(itemId);
                OnInventoryUpdated?.Invoke();
            }
        }

        public void RemoveItemAll(Item_Data_ item)
        {
            int itemId = item.GetId();
            RemoveItemAll(itemId);
        }

        public Dictionary<int,int> GetInventoryItemAll()
        {
            return new Dictionary<int, int>(inventoryItems);
        }

        public Dictionary<int, int> GetItemsByType(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Resource:
                    Dictionary<int, int> resourceItems = inventoryItems
                    // 1. Where: 키(k)가 minKey 이상(>=)이고 maxKey 이하(<=)인 항목만 필터링합니다.
                    .Where(kvp => kvp.Key >= resourceKeyStart && kvp.Key <= resourceKeyEnd)
                    // 2. ToDictionary: 필터링된 결과를 새로운 Dictionary로 변환합니다.
                    //    kvp.Key를 새 Dictionary의 Key로, kvp.Value를 새 Dictionary의 Value로 사용합니다.
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    return resourceItems;

                case ItemType.Facility:
                    Dictionary<int, int> facilityItems = inventoryItems
                    .Where(kvp => kvp.Key is >= facilityKeyStart and <= facilityKeyEnd)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    return facilityItems;
                case ItemType.Engine:
                    Dictionary<int, int> engineItems = inventoryItems
                    .Where(kvp => kvp.Key >= engineKeyStart && kvp.Key <= engineKeyEnd)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    return engineItems;

                default:
                    Debug.LogError($"Invalid ItemType provided: {itemType}");
                return new Dictionary<int, int>();
            }
        }

        public int GetItemCountById(int itemId)
        {
            if (inventoryItems.TryGetValue(itemId, out int count))
            {
                return count;
            }
            return 0;
        }

        public bool IsContainsItem( int itemId, int count )
        {
            return count <= GetItemCountById(itemId);
        }

        public bool HasItem(int itemId, int requiredCount = 1)
        {
            return GetItemCountById(itemId) >= requiredCount;
        }
        #endregion

        #region private methods
        private void SaveInventoryData()
        {
            SaveManager.Instance.SaveInventoryData(inventoryItems);
        }

        #endregion

        #region Unity event methods
        
        #endregion

    }
}
