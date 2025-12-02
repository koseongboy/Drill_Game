using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DrillGame.Core.Managers;
using DrillGame.Managers;
using DrillGame.UI;
using DrillGame.UI.Interface;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_EngineMerger : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties

        private int showingEngineItemId;
        
        [SerializeField] private TextMeshProUGUI ui_currentProcess;
        
        [SerializeField] private GameObject ui_piece;
        [SerializeField] private RectTransform contentRectTransform;
        [SerializeField] private RectTransform ui_newParent;
        [SerializeField] private RectTransform ui_combineParent;
        
        
        [SerializeField] private int engineMergerLevel;
        private List<int> ableEngineItemIds;

        private List<GameObject> newProcessPieces;
        private List<GameObject> combinedProcessPieces;

        // private int selectedRandomEngineLevel;

        private Action OnButtonPressed;
        #endregion

        #region Singleton & initialization
        public static UI_EngineMerger Instance { get; private set; }
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
        }
        
        private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address; 
        }
        #endregion

        #region getters & setters

        public void SetLevel(int level)
        {
            engineMergerLevel = level;
            LoadProcessList();
            UpdateProcessList();
        }
        #endregion

        #region public methods
        public virtual void CloseUI()
        {
            ClearAllPieces();
            CloseAction();
        }


        #endregion

        #region private methods
        
        private void UpdateCurrentProcess()
        {
            var type = EngineMergerManager.Instance.GetCurrentType();
            if (type == EngineMergerManager.MergeProcessingType.None)
            {
                ui_currentProcess.text = "현재 : 없음";
            }else if (type == EngineMergerManager.MergeProcessingType.Create)
            {
                ui_currentProcess.text = $"현재 : 신규 엔진 Lv.{engineMergerLevel}";
            }else if (type == EngineMergerManager.MergeProcessingType.Combine)
            {
                var target = EngineMergerManager.Instance.GetTargetEngineItemId();
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(target);

                ui_currentProcess.text = $"현재 : {itemData.DisplayName}";
            }
        }

        private void LoadProcessList()
        {
            ableEngineItemIds = new List<int>();
            var dict = ScriptableObjectManager.Instance.GetAllData<Item_Data_>();
            
            foreach (var kvp in dict)
            {
                var itemData = (Item_Data_)kvp.Value;
                var type = itemData.GetItemType_Enum();
                if (type == InventoryManager.ItemType.Engine)
                {
                    var engineData = ScriptableObjectManager.Instance.GetData<Engine_Data_>(itemData.EntityId);
                    if (engineData.Level >= 2 && engineData.Level <= engineMergerLevel+1)
                    {
                        ableEngineItemIds.Add(itemData.Id);
                    }
                }
            }
        }

        private void UpdateProcessList()
        {
            newProcessPieces = new List<GameObject>();
            combinedProcessPieces = new List<GameObject>();
            
            // 새 엔진 합성
            var newProcessPiece = Instantiate(ui_piece, ui_newParent);

            var name = $"신규 엔진 Lv.{engineMergerLevel}";
            var iconName = "engine_Test";
            newProcessPiece.GetComponent<UI_ContentPiece>()
                .SetData(name, iconName,
                    () => { OpenDetail( 0, engineMergerLevel ); });
            newProcessPieces.Add(newProcessPiece);

            foreach (var itemId in ableEngineItemIds)
            {
                var obj = Instantiate(ui_piece, ui_combineParent);
                
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(itemId);
                var engineData = ScriptableObjectManager.Instance.GetData<Engine_Data_>(itemData.EntityId);
                
                obj.GetComponent<UI_ContentPiece>()
                    .SetData(engineData.DisplayName, engineData.Icon,
                        () => { OpenDetail( 1, itemId ); });
                combinedProcessPieces.Add(obj);
            }
            
            StartCoroutine(UpdateLayout());
        }
        
        private IEnumerator UpdateLayout()
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
        }
        
        private void ClearAllPieces()
        {
            foreach (var obj in newProcessPieces)
            {
                Destroy(obj);
            }

            foreach (var obj in combinedProcessPieces)
            {
                Destroy(obj);
            }
        }
        
        private void CloseAction()
        {
            UILoader.Instance.HideUI(addressableName);
        }
        #endregion

        #region DetailWindow
                
        [SerializeField] private GameObject ui_detailWindow;
        [SerializeField] private TextMeshProUGUI ui_title;
        [SerializeField] private TextMeshProUGUI ui_desc;
        [SerializeField] private TextMeshProUGUI ui_selectButtonTxt;

        public void OpenDetail(int flag, int value)
        {
            ui_detailWindow.SetActive(true);
            
            if (flag == 0)
            {
                ui_title.text = $"새 엔진 생산 Lv.{engineMergerLevel}";
                ui_desc.text = $"새로운 Lv.{engineMergerLevel} 엔진을 랜덤으로 생산합니다.";
                ui_selectButtonTxt.text = "선택";
                OnButtonPressed = RegisterEngineToCreate;
                

                if (EngineMergerManager.Instance.GetCurrentType() == EngineMergerManager.MergeProcessingType.Create)
                {
                    ui_selectButtonTxt.text = "취소";
                    OnButtonPressed = StopProcess;
                }
            }else if (flag == 1)
            {
                var targetItemId = value;
                
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(targetItemId);
                var engineData = ScriptableObjectManager.Instance.GetData<Engine_Data_>(itemData.EntityId);

                showingEngineItemId = targetItemId;
                
                ui_title.text = $"합성 : {engineData.DisplayName}";
                ui_desc.text = $"낮은 레벨의 엔진 2개를 소모하여, {engineData.DisplayName} 하나를 생산합니다.";
                
                ui_selectButtonTxt.text = "선택";
                OnButtonPressed = RegisterEngineToCombine;

                if (EngineMergerManager.Instance.GetCurrentType() == EngineMergerManager.MergeProcessingType.Combine)
                {
                    if (EngineMergerManager.Instance.GetTargetEngineItemId() == showingEngineItemId)
                    {
                        ui_selectButtonTxt.text = "취소";
                        OnButtonPressed = StopProcess;
                    }
                }
            }

            DetailWindowOpenAnimation();
        }
        
        private void DetailWindowOpenAnimation()
        {
            ui_detailWindow.SetActive(true);
            RectTransform rt = ui_detailWindow.GetComponent<RectTransform>();
            
            Vector2 startPos = new Vector2(0, -500f);
            Vector2 targetPos = new Vector2(startPos.x, startPos.y + 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.OutBack);
            
            rt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(Vector2.one, 0.1f)
                .SetEase(Ease.OutBack);
        }
        
        public void CloseDetailWindow()
        {
            DetailWindow_Init();
            DetailWindowCloseAnimation();
        }

        private void DetailWindow_Init()
        {
            showingEngineItemId = 0;
            ui_title.text = "";
            ui_desc.text = "";
            ui_selectButtonTxt.text = "선택";
            OnButtonPressed = null;
        }

        private void DetailWindowCloseAnimation()
        {
            RectTransform rt = ui_detailWindow.GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y - 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.Linear);
            
            Vector3 targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(targetScale, 0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    ui_detailWindow.SetActive(false);
                });
        }
        
        public void DetailWindowSelectButtonPressed()
        {
            OnButtonPressed?.Invoke();
        }
        
        private void RegisterEngineToCreate()
        {
            EngineMergerManager.Instance.RegisterEngineToCreate(engineMergerLevel);
        }

        private void RegisterEngineToCombine()
        {
            var inputEngineId = showingEngineItemId - 1;
            Debug.Log(showingEngineItemId);
            
            EngineMergerManager.Instance.RegisterEngineToCombine(showingEngineItemId, inputEngineId);
        }

        private void StopProcess()
        {
            EngineMergerManager.Instance.StopEngineMergeProcess();
        }
        #endregion
        
        #region Unity event methods

        private void OnEnable()
        {
            DetailWindow_Init();
            ui_detailWindow.SetActive(false);

            EngineMergerManager.Instance.OnProcessChanged += UpdateCurrentProcess;
            UpdateCurrentProcess();
        }

        private void OnDisable()
        {
            EngineMergerManager.Instance.OnProcessChanged -= UpdateCurrentProcess;
        }


        #endregion


    }
}
