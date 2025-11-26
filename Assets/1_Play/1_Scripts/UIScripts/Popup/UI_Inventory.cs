using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotObjectParent;

        [SerializeField] private Dictionary<int, int> showingItemsCountDict = new Dictionary<int, int>();
        [SerializeField] private List<Item_Data_> showingItems = new List<Item_Data_>();

        // Slot Object Pooling
        private IObjectPool<GameObject> slotPool;
        [SerializeField] private List<GameObject> activeSlotObjects = new List<GameObject>();

        private int defaultPoolSize = 14;
        private int maxPoolSize = 80;

        public void LinkAddressable(string address)
        {
            addressableName = address;
        }

        public void ChangeInventoryType(InventoryManager.ItemType itemType)
        {
            LoadInventory(itemType);
            UpdateUI();
        }

        public void ChangeInventoryTypeByViewState(GameViewManager.ViewState viewState)
        {
            var itemType = viewState switch
            {
                GameViewManager.ViewState.EngineOnly => InventoryManager.ItemType.Engine,
                GameViewManager.ViewState.FacilityOnly => InventoryManager.ItemType.Facility,
                _ => InventoryManager.ItemType.None
            };
            LoadInventory(itemType);
            UpdateUI();
        }


        private void OpenAction()
        {

        }

        private void CloseAction()
        {

        }

        private void LoadInventory(InventoryManager.ItemType itemType = InventoryManager.ItemType.Facility)
        {
            showingItems.Clear();
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

            // Debug.Log($"LoadInventory : {itemType}, Count: {showingItems.Count}");
        }

        // NOTE : showingItems는 사전에 Update 되어있어야 함.
        // 보이는 UI를 변경하기만 함.
        private void UpdateUI()
        {
            foreach (var activeSlotObject in activeSlotObjects)
            {
                slotPool.Release(activeSlotObject);
            }

            activeSlotObjects.Clear();

            foreach (var itemData in showingItems)
            {
                if (slotPool == null)
                {
                    Debug.Log($"slotPool is null");
                }

                var slotObject = slotPool.Get();

                UI_ItemSlot uiItemSlot = slotObject.GetComponent<UI_ItemSlot>();
                uiItemSlot.SetItemData(itemData);
                activeSlotObjects.Add(slotObject);
            }
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
            LoadInventory(itemType);
            UpdateUI();
        }

        #region Pool Methods

        private GameObject CreatePooledItem()
        {
            var slot = Instantiate(slotPrefab, slotObjectParent, false);
            return slot;
        }

        private void OnTakeFromPool(GameObject slot)
        {
            slot.SetActive(true);
        }

        private void OnReturnToPool(GameObject slot)
        {
            slot.SetActive(false);
        }

        private void OnDestroyPoolObject(GameObject slot)
        {
            Destroy(slot);
        }

        #endregion

        private void Awake()
        {
            slotPool = new ObjectPool<GameObject>(
                CreatePooledItem, // 1. Create Action
                OnTakeFromPool, // 2. Get Action
                OnReturnToPool, // 3. Release Action
                OnDestroyPoolObject, // 4. Destroy Action
                // PoolSize 설정
                collectionCheck: false,
                defaultPoolSize,
                maxPoolSize
            );

            // 초기 풀링 개수를 미리 채웁니다 (선택 사항)
            // 풀에 채워지는 과정에서 CreatePooledItem -> OnReturnToPool이 호출됩니다.
            for (int i = 0; i < defaultPoolSize; i++)
            {
                slotPool.Release(CreatePooledItem());
            }
        }

        private void OnEnable()
        {
            var itemType = GameViewManager.Instance.GetViewState() switch
            {
                GameViewManager.ViewState.EngineOnly => InventoryManager.ItemType.Engine,
                GameViewManager.ViewState.FacilityOnly => InventoryManager.ItemType.Facility,
                _ => InventoryManager.ItemType.None
            };
            InventoryManager.Instance.OnInventoryUpdated += OnInventoryUpdated;

            LoadInventory(itemType);
            UpdateUI();
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
            
            UpdateUI();
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
