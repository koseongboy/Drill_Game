using System;
using System.Collections.Generic;
using DG.Tweening;
using DrillGame.Core.Managers;
using DrillGame.UI;
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
        
        private readonly Vector2 START_POSITION = new Vector2(-232, -184);
        private readonly Vector2 FINAL_POSITION = new Vector2(-232, -234); 
        #endregion

        #region Singleton & initialization
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        
        public static UI_ResourceInventory Instance { get; private set; }
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
            
            itemDict = new Dictionary<int, int>();
            activeObjects = new List<GameObject>();
        }
        
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        public void CloseUI()
        {
            CloseAction();
        }
        #endregion

        #region private methods
        private void OpenAction()
        {
            RectTransform rt = GetComponent<RectTransform>();
    
            rt.anchoredPosition = START_POSITION;
            rt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    
            rt.DOAnchorPos(FINAL_POSITION, 0.1f)
                .SetEase(Ease.InQuad);
    
            rt.DOScale(Vector2.one, 0.1f)
                .SetEase(Ease.InQuad);
        }

        private void CloseAction() {
            RectTransform rt = GetComponent<RectTransform>();
    
            rt.DOAnchorPos(START_POSITION, 0.1f)
                .SetEase(Ease.InQuad);
    
            Vector3 targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(targetScale, 0.1f)
                .SetEase(Ease.InQuad)
                .OnComplete(() => {
                    UILoader.Instance.HideUI(addressableName);
                });
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
            foreach (var kvp in itemDict)
            {
                var obj = Instantiate(slotPrefab, prefabParent);

                obj.GetComponent<UI_ResourceItemPiece>().UpdateUI( kvp.Key, kvp.Value );
                
                activeObjects.Add(obj);
            }
        }

        private void UpdateUI_ResourcePiece()
        {
            
        }
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            OpenAction();
            InventoryManager.Instance.OnInventoryUpdated += OnUpdateInventory;
            OnUpdateInventory();
        }

        private void OnDisable()
        {
            CloseAction();
            InventoryManager.Instance.OnInventoryUpdated -= OnUpdateInventory;
        }

        #endregion


    }
}