using System;
using System.Collections.Generic;
using System.Linq;
using DrillGame._1_Play._1_Scripts.Managers.Mono;
using DrillGame.Core.Managers;
using DrillGame.Managers;
using DrillGame.UI.Interface;
using UnityEngine;
using UnityEngine.Pool;

namespace DrillGame
{
    public class UI_Inventory : MonoBehaviour, UI_IAddressable
    {
        [SerializeField] private string addressableName;

        private Dictionary<int, int> showingItemsCountDict = new Dictionary<int, int>(); 
        [SerializeField] private RectTransform inventoryContent;

        private List<Item_Data_> showingItems = new List<Item_Data_>();
        // Slot Object Pooling
        private List<GameObject> activeSlotObjects = new List<GameObject>();

        
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        
        
        private void OpenAction()
        {

        }

        private void CloseAction()
        {
            Init();
        }
        
        // Inventory Manager 옵저빙
        private void OnInventoryUpdated()
        {
            var itemType = GameViewManager.Instance.GetViewState() switch
            {
                GameViewManager.ViewState.EngineOnly => InventoryManager.ItemType.Engine,
                GameViewManager.ViewState.FacilityOnly => InventoryManager.ItemType.Facility,
                _ => InventoryManager.ItemType.None
            };
            UpdateUI(itemType);
        }
        
        public void ChangeInventoryTypeByViewState(GameViewManager.ViewState viewState)
        {
            var itemType = viewState switch
            {
                GameViewManager.ViewState.EngineOnly => InventoryManager.ItemType.Engine,
                GameViewManager.ViewState.FacilityOnly => InventoryManager.ItemType.Facility,
                _ => InventoryManager.ItemType.None
            };
            UpdateUI(itemType);
        }
        
        private void UpdateUI(InventoryManager.ItemType itemType)
        {
            Init();
            LoadInventory(itemType);
            UpdateUI_ItemSlotPieces();
        }

        private void Init()
        {
            showingItemsCountDict = new Dictionary<int, int>();
            showingItems = new List<Item_Data_>();
            foreach (var obj in activeSlotObjects)
            {
                ItemSlotPoolManager.Instance.Return(obj);
            }
            activeSlotObjects = new List<GameObject>();
        }

        private void LoadInventory(InventoryManager.ItemType itemType = InventoryManager.ItemType.Facility)
        {
            showingItemsCountDict = InventoryManager.Instance.GetItemsByType(itemType);

            foreach (var kvp in showingItemsCountDict)
            {
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(kvp.Key);
                if (itemData == null)
                {
                    Debug.LogWarning($"ItemData is null for ItemID: {kvp.Key}");
                    continue;
                }

                if (itemData.GetItemType_Enum() == itemType)
                {
                    for (int i = 0; i < kvp.Value; i++)
                    {
                        showingItems.Add(itemData);
                    }
                }
            }
        }

        // NOTE : showingItems는 사전에 Update 되어있어야 함.
        // 보이는 UI를 변경하기만 함.
        private void UpdateUI_ItemSlotPieces()
        {
            foreach (var itemData in showingItems)
            {
                var slotObject = ItemSlotPoolManager.Instance.Get();

                UI_ItemSlot uiItemSlot = slotObject.GetComponent<UI_ItemSlot>();
                uiItemSlot.SetItemData(itemData);
                activeSlotObjects.Add(slotObject);
            }
        }


        private void OnEnable()
        {
            InventoryManager.Instance.OnInventoryUpdated += OnInventoryUpdated;
            OnInventoryUpdated();
            OpenAction();
        }

        #region DEV

        public static void PrintAll_Dict<T, S>(Dictionary<T, S> dict)
        {
            string str ="";
            foreach (var kvp in dict)
            {
                str += $"{kvp.Key}: {kvp.Value}  |  ";
            }
            Debug.Log(str);
        }
        
        [ContextMenu("UpdateUITest")]

        public void UpdateUITest()
        {
            showingItems.Clear();
            showingItems.Add( ScriptableObjectManager.Instance.GetData<Item_Data_>(1001) );
            showingItems.Add( ScriptableObjectManager.Instance.GetData<Item_Data_>(1002) );
            showingItems.Add( ScriptableObjectManager.Instance.GetData<Item_Data_>(1003) );
            showingItems.Add( ScriptableObjectManager.Instance.GetData<Item_Data_>(1004) );
            showingItems.Add( ScriptableObjectManager.Instance.GetData<Item_Data_>(1005) );
            
            UpdateUI_ItemSlotPieces();
        }
        
        [ContextMenu("AddUnitItems_DEV")]
        public void AddUnitItems_DEV()
        {
            AddFacilityItems_DEV();
            AddEngineItems_DEV();
        }
        
        [ContextMenu("AddFacilityItems_DEV")]
        public void AddFacilityItems_DEV()
        {
            InventoryManager.Instance.AddItem( 1201 );
            InventoryManager.Instance.AddItem( 1201 );
            InventoryManager.Instance.AddItem( 1301 );
            InventoryManager.Instance.AddItem( 1301 );
        }
        
        [ContextMenu("AddEngineItems_DEV")]
        public void AddEngineItems_DEV()
        {
            InventoryManager.Instance.AddItem( 1104 );
            InventoryManager.Instance.AddItem( 1104 );
            InventoryManager.Instance.AddItem( 1104 );
        }
        #endregion
    }
}
