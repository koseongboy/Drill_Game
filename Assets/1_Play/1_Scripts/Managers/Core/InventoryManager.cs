using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace DrillGame.Core.Managers
{
    public class InventoryManager
    {
        #region Fields & Properties
        private List<ItemData> inventoryItems = new List<ItemData>();

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
                }
                return instance;
            }
        }


        #endregion

        #region getters & setters
        #endregion

        #region public methods
        public void AddItem(ItemData item)
        {
            inventoryItems.Add(item);
            Debug.Log($"Item added: {item.displayName}");
            OnInventoryUpdated?.Invoke();
        }

        public void RemoveItem(ItemData item)
        {
            if (inventoryItems.Remove(item))
            {
                Debug.Log($"Item removed: {item.displayName}");
            }
            else
            {
                Debug.LogWarning($"Item not found in inventory: {item.displayName}");
            }
            OnInventoryUpdated?.Invoke();
        }

        public List<ItemData> GetInventoryItemAll()
        {
            return new List<ItemData>(inventoryItems);
        }

        public List<ItemData> GetItemsByType(string itemType)
        {
            return inventoryItems.FindAll(item => item.itemType == itemType);
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion




    }
}
