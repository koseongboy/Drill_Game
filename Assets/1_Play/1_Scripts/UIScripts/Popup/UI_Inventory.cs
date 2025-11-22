using System;
using System.Collections.Generic;
using DrillGame.Core.Managers;
using DrillGame.Managers;
using DrillGame.UI.Interface;
using UnityEngine;
using UnityEngine.Pool;

namespace DrillGame
{
    public class UI_Inventory : MonoBehaviour, UI_IAddressable
    {
        [SerializeField]
        private string addressableName;
        [SerializeField]
        private GameObject slotPrefab;
        [SerializeField]
        private Transform slotObjectParent;
        
        private List<GameObject> slotObjects = new List<GameObject>();
        private List<Item_Data_> showingItems = new List<Item_Data_>();
        
        // Slot Object Pooling
        private IObjectPool<GameObject> slotPool;
        private readonly List<GameObject> activeSlotObjects = new List<GameObject>();
        
        private int defaultPoolSize = 19;
        private int maxPoolSize = 80;
        
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }

        private void OpenAction()
        {

        }

        private void CloseAction() {

        }
        
        private void LoadInventory()
        {
            var viewState = GameViewManager.Instance.GetViewState();
            viewState = GameViewManager.ViewState.All; // DEV
            InventoryManager.ItemType itemType = viewState switch
            {
                GameViewManager.ViewState.EngineOnly => InventoryManager.ItemType.Engine,
                GameViewManager.ViewState.FacilityOnly => InventoryManager.ItemType.Facility,
                _ => InventoryManager.ItemType.None
            };

            showingItems.Clear();
            showingItems = InventoryManager.Instance.GetItemsByType(itemType);
        }
        private void ClearSlots()
        {
            foreach (var activeSlotObject in activeSlotObjects)
            {
                slotPool.Release(activeSlotObject);
            }
            slotObjects.Clear();
        }
        private void UpdateUI()
        {
            ClearSlots();
            foreach (var itemData in showingItems)
            {
                var slotObject = slotPool.Get();
                slotObject.transform.SetParent(slotObjectParent);
                var count = 1; // TODO
                
                UI_ItemSlot uiItemSlot = slotObject.GetComponent<UI_ItemSlot>();
                uiItemSlot.SetItemData(itemData, count);
                activeSlotObjects.Add(slotObject);
            }
        }
        
        #region Pool Methods
        private GameObject CreatePooledItem()
        {
            var slot = Instantiate(slotPrefab, this.transform, false);
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
                collectionCheck: false, // 풀에서 빼고 넣는 과정의 에러 체크 (프로덕션에서는 false로 성능 개선)
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
            LoadInventory();
            UpdateUI();
            OpenAction();
        }


        #region DEV
        [ContextMenu("Test")]
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
        #endregion
    }
}
