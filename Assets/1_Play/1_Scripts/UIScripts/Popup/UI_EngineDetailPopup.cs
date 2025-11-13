using DG.Tweening;
using DrillGame.Core.Engine;
using DrillGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

using DrillGame.UI.Interface;

namespace DrillGame
{
    public class UI_EngineDetailPopup : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField]
        private string addressableName;
        #endregion

        #region getters & setters
        #endregion

        #region public methods

        public void UpdateDetail(EngineEntity entity)
        {
            
        }
        public void CloseUI()
        {
            // Debug.Log($"{gameObject.name}: UI 종료 시도, addressable 주소 : {addressableName}");
            CloseAction();
        }
        
        public void LinkAddressable(string address)
        {
            Debug.Log($"{gameObject.name}: addressable 주소 설정 : {address}");
            addressableName = address;
        }
        #endregion

        #region private methods
        
        private void OpenAction()
        {
            RectTransform rt = GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y + 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.OutBack);
            
            rt.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(Vector2.one, 0.1f)
                .SetEase(Ease.OutBack);
        }

        private void CloseAction() {
            RectTransform rt = GetComponent<RectTransform>();
            
            Vector2 startPos = rt.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, startPos.y - 100f);
            rt.anchoredPosition = startPos;
            rt.DOAnchorPos(targetPos, 0.1f)
                .SetEase(Ease.Linear);
            
            Vector3 targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            rt.DOScale(targetScale, 0.1f)
                .SetEase(Ease.Linear)
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
