using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace DrillGame.Core.Managers
{
    public class InventoryManager
    {
        #region Fields & Properties
        private List<Item_Data_> inventoryItems = new List<Item_Data_>();

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
        public void AddItem(Item_Data_ item)
        {
            inventoryItems.Add(item);
            Debug.Log($"Item added: {item.DisplayName}");
            OnInventoryUpdated?.Invoke();
        }

        public void RemoveItem(Item_Data_ item)
        {
            if (inventoryItems.Remove(item))
            {
                Debug.Log($"Item removed: {item.DisplayName}");
            }
            else
            {
                Debug.LogWarning($"Item not found in inventory: {item.DisplayName}");
            }
            OnInventoryUpdated?.Invoke();
        }

        public List<Item_Data_> GetInventoryItemAll()
        {
            return new List<Item_Data_>(inventoryItems);
        }

        public List<Item_Data_> GetItemsByType(string itemType)
        {
            return inventoryItems.FindAll(item => item.ItemType == itemType);
        }
        #endregion

        #region private methods
        #endregion

        #region Unity event methods
        #endregion




    }
}
