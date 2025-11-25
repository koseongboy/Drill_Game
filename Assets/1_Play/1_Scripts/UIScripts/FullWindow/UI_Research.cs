using System;
using DG.Tweening;
using DrillGame.UI;
using DrillGame.UI.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame._1_Play._1_Scripts.UIScripts.FullWindow
{
    public class UI_Research : MonoBehaviour, UI_IAddressable
    {
        #region Singleton & initialization
        public static UI_Research Instance { get; private set; }
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
        #endregion
        
        #region UI_Addressable
        [SerializeField]
        private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        #endregion
        
        #region Fields & Properties

        [SerializeField] private GameObject detailWindow;
        
        [SerializeField]
        private TextMeshProUGUI researchNameTxt;
        [SerializeField]
        private Image progressBar;
        [SerializeField]
        private TextMeshProUGUI progressTxt;
        [SerializeField]
        private TextMeshProUGUI researchDescTxt;
        [SerializeField]
        private TextMeshProUGUI researchInputItemTxt;
        [SerializeField]
        private TextMeshProUGUI selectButtonTxt;
        
        private Action<int> SelectButtonPressedAction;

        private int showingResearchId;
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        public virtual void CloseUI()
        {
            CloseAction();
        }

        public void OpenDetailWindow(int researchId)
        {
            showingResearchId = researchId;
            ResearchManager.Instance.OnResearchProgressChanged += OnUpdateResearchProgress;
            UpdateDetailWindow( researchId );
            DetailWindowOpenAnimation();
        }

        public void CloseDetailWindow()
        {
            ResearchManager.Instance.OnResearchProgressChanged += OnUpdateResearchProgress;
            DetailWindowInit();
            DetailWindowCloseAnimation();
        }

        public void SelectButtonPressed() {
            Debug.Log($"SelectButtonPressed : { showingResearchId }");
            SelectButtonPressedAction?.Invoke( showingResearchId );
            UpdateDetailWindow( showingResearchId );
        }
        #endregion

        #region private methods

        private void OpenAction()
        {
            
        }
        private void CloseAction()
        {
            UILoader.Instance.HideUI(addressableName);
        }

        private void DetailWindowInit() {
            showingResearchId = 0;
            researchNameTxt.text = string.Empty;
            progressBar.fillAmount = 0;
            progressTxt.text = string.Empty;
            researchDescTxt.text = string.Empty;
            researchInputItemTxt.text = string.Empty;
            SelectButtonPressedAction = null;
        }

        private void UpdateDetailWindow( int researchId ) {
            Research_Data_ data = ScriptableObjectManager.Instance.GetData<Research_Data_>( researchId );
            float progress = ResearchManager.Instance.GetResearchProgress( researchId );
            
            // Name
            researchNameTxt.text = data.DisplayName;
            
            // Progress
            float progressRate = progress / data.ResearchAmount;
            progressBar.fillAmount = progressRate;
            progressTxt.text = $"현재 진척도 : {progress}/{data.ResearchAmount} ({(progressRate * 100):F1}%)";
            
            // Desc
            researchDescTxt.text = data.Desc;
            
            // Input Item
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(
                data.InputItemPerTickId);
            researchInputItemTxt.text = $"필요 자원 : 틱 당 {itemData.DisplayName} {data.InputItemPerTickCount}개";

            // Button Update
            SelectButtonPressedAction = null;
            if (!IsResearchUnLocked( researchId )) {
                selectButtonTxt.text = "잠김";
            }else if (!IsResearchSelected( researchId )) {
                selectButtonTxt.text = "선택";
                SelectButtonPressedAction += SelectResearch;
            }
            else {
                selectButtonTxt.text = "선택 해제";
                SelectButtonPressedAction += UnSelectResearch;
            }
        }

        private void OnUpdateResearchProgress(int researchId, float progress,float progressRate) {
            if (showingResearchId != researchId) {
                return;
            }
            
            Research_Data_ data = ScriptableObjectManager.Instance.GetData<Research_Data_>( researchId );
            progressBar.fillAmount = progressRate;
            progressTxt.text = $"현재 진척도 : {progress:F1}/{data.ResearchAmount} ({(progressRate * 100):F1}%)";
        }

        private void DetailWindowOpenAnimation()
        {
            detailWindow.SetActive(true);
            RectTransform rt = detailWindow.GetComponent<RectTransform>();
            
            Vector2 startPos = new Vector2(0, -500f);
            Vector2 targetPos = new Vector2(startPos.x, startPos.y + 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.OutBack);
            
            rt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(Vector2.one, 0.1f)
                .SetEase(Ease.OutBack);
        }
        private void DetailWindowCloseAnimation()
        {
            RectTransform rt = detailWindow.GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y - 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.Linear);
            
            Vector3 targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(targetScale, 0.1f)
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    detailWindow.SetActive(false);
                });
        }

        private void SelectResearch(int researchId) {
            ResearchManager.Instance.SelectResearch( researchId );
        }

        private void UnSelectResearch(int researchId) {
            ResearchManager.Instance.UnSelectResearch();
        }

        private bool IsResearchSelected(int researchId) {
            var selectedResearchId = ResearchManager.Instance.GetSelectedResearchId();
            return selectedResearchId == researchId;
        }
        
        private bool IsResearchUnLocked( int researchId ) {
            return ResearchManager.Instance.IsResearchUnLocked(researchId);
        }
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            OpenAction();
            DetailWindowInit();
            detailWindow.SetActive(false);
        }

        #endregion
    }
}