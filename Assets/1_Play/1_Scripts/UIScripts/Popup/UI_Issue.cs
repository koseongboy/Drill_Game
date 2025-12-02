using DG.Tweening;
using DrillGame.UI;
using DrillGame.UI.Interface;
using UnityEngine;

namespace DrillGame
{
    public class UI_Issue : MonoBehaviour, UI_IAddressable
    {
        #region Fields & Properties
        
        private readonly Vector2 START_POSITION = new Vector2(-636, -172);
        private readonly Vector2 FINAL_POSITION = new Vector2(-636, -272); 

        #endregion

        #region Singleton & initialization
        [SerializeField] private string addressableName;
        public void LinkAddressable(string address)
        {
            addressableName = address;
        }
        #endregion

        #region getters & setters
        public void CloseUI()
        {
            CloseAction();
        }
        #endregion

        #region public methods
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
        #endregion

        #region Unity event methods

        private void OnEnable()
        {
            OpenAction();
        }
        #endregion
    }
}
