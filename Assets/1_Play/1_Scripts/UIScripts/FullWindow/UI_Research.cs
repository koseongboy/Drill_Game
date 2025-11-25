using DG.Tweening;
using DrillGame.UI;
using DrillGame.UI.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrillGame._1_Play._1_Scripts.UIScripts.FullWindow
{
    public class UI_Research : MonoBehaviour ,UI_IAddressable
    {
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
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        public void OpenDetail(int researchId)
        {
            Research_Data_ data = ScriptableObjectManager.Instance.GetData<Research_Data_>( researchId );
            float progress = ResearchManager.Instance.GetResearchProgress( researchId );
            
            // Name
            researchNameTxt.text = data.DisplayName;
            
            // Progress
            float progressRate = progress / data.ResearchAmount;
            progressBar.fillAmount = progressRate;
            progressTxt.text = $"현재 진척도 : {progress}/{data.ResearchAmount} ({progressRate}%)";
            
            // Desc
            researchDescTxt.text = data.Desc;
            
            // Input Item
            var itemData = ScriptableObjectManager.Instance.GetData<Item_Data_>(
                data.InputItemPerTickId);
            researchInputItemTxt.text = $"필요 자원 : 틱 당 {itemData.DisplayName} {data.InputItemPerTickCount}개";
            
            // Button Update
            // TODO

            DetailWindowOpenAction();
        }

        public void CloseDetail()
        {
            researchNameTxt.text = string.Empty;
            progressBar.gameObject.SetActive(false);
            progressTxt.text = string.Empty;
            researchDescTxt.text = string.Empty;
            researchInputItemTxt.text = string.Empty;
            DetailWindowCloseAction();
        }
        #endregion

        #region private methods

        private void DetailWindowOpenAction()
        {
            detailWindow.SetActive(true);
            RectTransform rt = detailWindow.GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y + 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.OutBack);
            
            rt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(Vector2.one, 0.1f)
                .SetEase(Ease.OutBack);
        }
        private void DetailWindowCloseAction()
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
        #endregion

        #region Unity event methods
        #endregion
    }
}