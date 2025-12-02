using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DrillGame.Managers;
using DrillGame.UI;
using DrillGame.UI.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame
{
    public class UI_ResourceConverter : MonoBehaviour, UI_IAddressable
    {
        #region Singleton & initialization
                
        private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address; 
        }
        #endregion
        
        #region Fields & Properties
        [SerializeField] private RectTransform contentRectTransform;
        [SerializeField] private GameObject ui_piece;
        [SerializeField] private RectTransform ui_pieceParent;
        
        [SerializeField] private TextMeshProUGUI ui_currentProcess;
        
        private List<int> researchIds = new List<int>
        {
            30001,
            30006,
            30011,
            30016,
            30021,
            30026
        };
        private List<int> unlockedOutputResourceIds = new List<int>();
        private List<GameObject> resourcePieces = new List<GameObject>();

        private Action OnButtonPressed;
        #endregion
        
        #region getters & setters
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
            var itemId = ResourceConverter.Instance.GetCurrentOutputItemId();
            if (itemId == 0)
            {
                ui_currentProcess.text = "현재 : 없음";
            }
            else
            {
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(itemId);
                ui_currentProcess.text = $"현재 : {itemData.DisplayName}";
            }
        }

        private void LoadOutputResourceList()
        {
            for (int i = 0; i < researchIds.Count; i++)
            {
                var researchId = researchIds[i];
                if (!ResearchManager.Instance.IsResearchDone(researchId))
                {
                    break;
                }
                
                unlockedOutputResourceIds.Add(1003 + i);
                unlockedOutputResourceIds.Add(1004 + i);
            }
        }

        private void UpdateUI_ResourceList()
        {
            resourcePieces = new List<GameObject>();
            
            // 새 자원
            foreach (var resourceId in unlockedOutputResourceIds)
            {
                var resourcePiece = Instantiate(ui_piece, ui_pieceParent);
                var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(resourceId);
                
                resourcePiece.GetComponent<UI_ContentPiece>()
                    .SetData( itemData.DisplayName, itemData.ItemIcon,
                        () => { OpenDetail(resourceId, resourceId-2); });
                resourcePieces.Add(resourcePiece); 
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
            foreach (var obj in resourcePieces)
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

        private int showingItemId;
        [SerializeField] private GameObject ui_detailWindow;
        [SerializeField] private TextMeshProUGUI ui_title;
        [SerializeField] private TextMeshProUGUI ui_desc;
        [SerializeField] private TextMeshProUGUI ui_selectButtonTxt;

        public void OpenDetail(int outputItemId, int inputItemId)
        {
            ui_detailWindow.SetActive(true);
         
            showingItemId = outputItemId;
            var outputItemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(outputItemId);
            var inputItemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(inputItemId);
            ui_title.text = outputItemData.DisplayName;
            ui_desc.text = $"{inputItemData.DisplayName} {ResourceConverter.Instance.GetInputItemCount()}개를 소모해서\n" +
                           $"{outputItemData.DisplayName} {ResourceConverter.Instance.GetOutputItemCount()}개를 생산한다.";
            ui_selectButtonTxt.text = "선택";
            OnButtonPressed = RegisterResoucreToConvert;


            if (ResourceConverter.Instance.GetCurrentOutputItemId() == outputItemId)
            {
                ui_selectButtonTxt.text = "취소";
                OnButtonPressed = StopProcess;
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
            showingItemId = 0;
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
        
        private void RegisterResoucreToConvert()
        {
            ResourceConverter.Instance.SetOutputItemId( showingItemId );
        }

        private void StopProcess()
        {
            ResourceConverter.Instance.SetOutputItemId( 0 );
        }
        #endregion
        

        #region Unity event methods

        private void OnEnable()
        {
            ResourceConverter.Instance.OnProcessChanged += UpdateCurrentProcess;
            UpdateCurrentProcess();
            LoadOutputResourceList();
            UpdateUI_ResourceList();
        }

        private void OnDisable()
        {
            ResourceConverter.Instance.OnProcessChanged -= UpdateCurrentProcess;
        }

        #endregion
    }
}