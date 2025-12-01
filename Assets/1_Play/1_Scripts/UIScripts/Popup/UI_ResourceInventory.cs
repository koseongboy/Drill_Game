using System;
using System.Collections.Generic;
using DrillGame.Core.Managers;
using DrillGame.UI.Interface;
using UnityEngine;

namespace DrillGame
{
    public class UI_ResourceInventory : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties
        [SerializeField] private string addressableName;

        private Dictionary<int, int> itemDict;
        private List<GameObject> activeObjects;

        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private RectTransform prefabParent;
        #endregion

        #region Singleton & initialization
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        #endregion

        #region getters & setters
        #endregion

        #region public methods
        #endregion

        #region private methods
        private void OpenAction()
        {

        }

        private void CloseAction()
        {
        }

        private void OnUpdateInventory()
        {
            Init();
            LoadInventory();
            UpdateUI();
        }

        private void Init()
        {
            itemDict = new Dictionary<int, int>();
            foreach (var obj in activeObjects)
            {
                Destroy(obj);
            }
            activeObjects = new List<GameObject>();
        }

        private void LoadInventory()
        {
            itemDict = InventoryManager.Instance.GetItemsByType( InventoryManager.ItemType.Resource );
        }

        private void UpdateUI()
        {
            foreach (var key in itemDict.Keys)
            {
                var obj = Instantiate(slotPrefab, prefabParent);
                // 여기서 Piece 개별 업데이트
                activeObjects.Add(obj);
            }
        }
        #endregion

        #region Unity event methods
        private void Awake()
        {
            itemDict = new Dictionary<int, int>();
            activeObjects = new List<GameObject>();
        }

        private void OnEnable()
        {
            
        }

        #endregion


    }
}